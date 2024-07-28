using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ABlock : MonoBehaviour
{
    protected ABlock parent;
    protected Dictionary<int, ABlock> childLinks = new();
    [SerializeField] private GameObject _mechaPrefab;
    [SerializeField] protected Transform[] holes;

    private void Awake()
    {
        Health health = GetComponent<Health>();
        if (health != null)
        {
            health.OnDestroyed += OnDestroyed;
        }
    }

    public virtual ABlock AddBlock(GameObject blockPrefab, int holeIndex)
    {
        if (childLinks.ContainsKey(holeIndex) || holeIndex >= holes.Length) return null;

        GameObject g = Instantiate(blockPrefab, holes[holeIndex]);
        g.transform.parent = transform;
        g.transform.localPosition = holes[holeIndex].localPosition;
        g.transform.localRotation = holes[holeIndex].localRotation;
        ABlock b = g.GetComponent<ABlock>();
        childLinks.Add(holeIndex, b);

        return b;
    }

    public virtual void UnlinkChildBlock(ABlock block)
    {
        if (childLinks.ContainsValue(block))
        {
            foreach (KeyValuePair<int, ABlock> pair in childLinks)
            {
                if (pair.Value == block)
                {
                    pair.Value.SetRoot();
                    childLinks.Remove(pair.Key);
                    break;
                }
            }
        }
    }

    public virtual void UnlinkChildBlockAt(int index)
    {
        if (childLinks.ContainsKey(index))
        {
            childLinks[index].SetRoot();
            childLinks.Remove(index);
        }
    }

    private void SetRoot()
    {
        transform.parent = null;
        GameObject g = Instantiate(_mechaPrefab, transform.position, transform.rotation);
        Mechanic m = g.GetComponent<Mechanic>();

        m.SetHeadBlock(this);
    }

    private void OnDestroyed()
    {
        foreach (var pair in childLinks)
        {
            UnlinkChildBlockAt(pair.Key);
            GameObject g = Instantiate(_mechaPrefab, pair.Value.transform.position, pair.Value.transform.rotation);
            Mechanic mechanic = g.GetComponent<Mechanic>();
            mechanic.SetHeadBlock(pair.Value);
        }
    }
}
