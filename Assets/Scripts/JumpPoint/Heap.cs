using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Heap<T> where T : IHeapItem<T>
{
    private T[] _items = null;
    private int _currentItemCount = 0;
    public int Count
    {
        get
        {
            return _currentItemCount;
        }
    }

    public Heap(int MaxHeapSize)
    {
        _items = new T[MaxHeapSize];
    }

    public T[] ToArray()
    {
        return _items;
    }

    public void Add(T item)
    {
        item.HeapIndex = _currentItemCount;
        _items[_currentItemCount] = item;
        _SortUp(item);
        _currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = _items[0];
        _currentItemCount--;
        _items[0] = _items[_currentItemCount];
        _items[0].HeapIndex = 0;
        _SortDown(_items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        _SortUp(item);
    }

    public bool Contains(T item)
    {
        return Equals(_items[item.HeapIndex], item);
    }

    private void _SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = _items[parentIndex];

            if (item.CompareTo(parentItem) > 0)
                _Swap(item, parentItem);
            else
                break;

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void _SortDown(T item)
    {
        while (true)
        {
            int childLeftIndex = (item.HeapIndex * 2) + 1;
            int childRightIndex = (item.HeapIndex * 2) + 2;
            int swapIndex = 0;

            if (childLeftIndex < _currentItemCount)
            {
                swapIndex = childLeftIndex;

                if (childRightIndex < _currentItemCount)
                    if (_items[childLeftIndex].CompareTo(_items[childRightIndex]) < 0)
                        swapIndex = childRightIndex;

                if (item.CompareTo(_items[swapIndex]) < 0)
                    _Swap(item, _items[swapIndex]);
                else
                    return;
            }
            else
                return;
        }
    }

    private void _Swap (T itemA, T itemB)
    {
        _items[itemA.HeapIndex] = itemB;
        _items[itemB.HeapIndex] = itemA;

        int tempItemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = tempItemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get; set;
    }
}