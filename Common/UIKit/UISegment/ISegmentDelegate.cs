

using UnityEngine;
using System.Collections;

/// <summary>
/// An interface for a data source to a TableView
/// </summary>
public interface ISegmentItemDelegate
{
    /// <summary>
    /// Get the number of rows that a certain table should display
    /// </summary>

    void SegmentItemDidClick(SegmentItem item);


}

 

public interface ISegmentDelegate
{
    /// <summary>
    /// Get the number of rows that a certain table should display
    /// </summary>

    void SegmentDidClick(UISegment seg,SegmentItem item);


}


