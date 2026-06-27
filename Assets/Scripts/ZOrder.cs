using UnityEngine;
using UnityEngine.Rendering;

public class ZOrder : MonoBehaviour
{
    public Transform TF;
    public SpriteRenderer SR;
    public SortingGroup SG;
    public int curOrder = -99;
    private Transform m_tf;
    

    void Start()
    {
        if (TF != null) m_tf = TF;
        else if (SG != null) m_tf = SG.transform;
        else m_tf = SR.transform; 
    }

    void Update()
    {
        if (m_tf == null) return;
        var order = Mathf.RoundToInt(-m_tf.position.y*100);
        if (order == curOrder) return;
        curOrder = order;
        if (SG != null) SG.sortingOrder = curOrder;
        else SR.sortingOrder = curOrder;
    }
}
