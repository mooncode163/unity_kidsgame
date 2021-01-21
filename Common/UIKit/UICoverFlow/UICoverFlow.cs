using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

//参考 https://github.com/7732050/UnityCoverFlow
public class UICoverFlow : UIView
{
    public enum Direction
    {
        Horizontal=0,
        Vertical,
    }
    [SerializeField]
    private UICollectionView m_coverFlow;

    [SerializeField]
    private Color[] m_colourData;

    [SerializeField]
    private int m_numberOfCells = 10;

    [SerializeField]
    private int m_groupSizes = 1;//
    public Direction direction;

    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        // iCollectionViewCell
        m_coverFlow.Init(PrefabCache.main.LoadByKey("UICellCover").GetComponent<iCollectionViewCell>());
        if (m_coverFlow.m_cell == null)
        {
            Debug.Log("m_coverFlow.m_cell==null");
        }
        else
        {
            Debug.Log("m_coverFlow.m_cell not null");
        }
        LayOut();
    }
    // Use this for initialization
    public void Start()
    {
        base.Start();

        //Build cells
        if (m_coverFlow != null)
        {
            //Build a bunch of cells - pass in data
            List<QuadCell.QuadCellData> data = new List<QuadCell.QuadCellData>();
            for (int i = 0; i < m_numberOfCells; i++)
            {
                if (m_colourData != null && m_colourData.Length > 0)
                {
                    data.Add(new QuadCell.QuadCellData() { MainColor = m_colourData[(i / m_groupSizes) % m_colourData.Length] });
                }
                else
                {
                    data.Add(new QuadCell.QuadCellData());
                }

            }

            //Bleugh
            m_coverFlow.Data = new List<object>(data.ToArray());
        }

        LayOut();
    }

}
