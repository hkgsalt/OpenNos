﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.GameObject
{
    class SortedCostMapCellList
    {

            ArrayList _list;
        MapCellComparer _cellComparer;


            public int Count
            {
                get
                {
                    return _list.Count;
                }
            }




            public SortedCostMapCellList()
            {
                _list = new ArrayList();
                _cellComparer = new MapCellComparer();
            }


            public MapCell CellAt(int i)
            {
                return (MapCell)_list[i];
            }

            public void RemoveAt(int i)
            {
                _list.RemoveAt(i);
            }

            public int IndexOf(MapCell n)
            {
                for (int i = 0; i < _list.Count; i++)
                {
                MapCell cellInTheList = (MapCell)_list[i];
                    if (cellInTheList.isMatch(n))
                        return i;
                }
                return -1;
            }

            public int push(MapCell n)
            {

                int k = _list.BinarySearch(n, _cellComparer);

                if (k == -1) 
                    _list.Insert(0, n);
                else if (k < 0) 
                {
                    k = ~k;
                    _list.Insert(k, n);
                }
                else if (k >= 0)
                    _list.Insert(k, n);

                return k;
            }

            public MapCell pop()
            {
            MapCell r = (MapCell)_list[0];
                _list.RemoveAt(0);
                return r;
            }


        }
    
}
