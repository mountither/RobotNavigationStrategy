using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{   
    /*
     * A PriorityQueue of generic type. will be used by GBFS and ASTAR since they need to order their frontiers based
     * on their respective costs. 
     * An IComparable interface is used at CellState to compare Costs. 
     */
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> fData;

        public PriorityQueue()
        {
            fData = new List<T>();
        }

        public void Enqueue(T item)
        {
            fData.Add(item);
            int ci = fData.Count - 1; // child index; start at end
            while (ci > 0)
            {
                int pi = (ci - 1) / 2; // parent index
                if (fData[ci].CompareTo(fData[pi]) >= 0) break; // child item is larger than (or equal) parent so we're done
                T tmp = fData[ci]; fData[ci] = fData[pi]; fData[pi] = tmp;
                ci = pi;
            }
        }

        public T Dequeue()
        {
            // assumes pq is not empty; up to calling code
            int li = fData.Count - 1; // last index (before removal)
            T frontItem = fData[0];   // fetch the front
            fData[0] = fData[li];
            fData.RemoveAt(li);

            --li; // last index (after removal)
            int pi = 0; // parent index. start at front of pq
            while (true)
            {
                int ci = pi * 2 + 1; // left child index of parent
                if (ci > li) break;  // no children so done
                int rc = ci + 1;     // right child
                if (rc <= li && fData[rc].CompareTo(fData[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                    ci = rc;
                if (fData[pi].CompareTo(fData[ci]) <= 0) break; // parent is smaller than (or equal to) smallest child so done
                T tmp = fData[pi]; fData[pi] = fData[ci]; fData[ci] = tmp; // swap parent and child
                pi = ci;
            }
            return frontItem;
        }

        public T Peek()
        {
            T frontItem = fData[0];
            return frontItem;
        }

        public int Count()
        {
            return fData.Count;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < fData.Count; ++i)
                s += fData[i].ToString() + " ";
            s += "count = " + fData.Count;
            return s;
        }

        public bool IsConsistent()
        {
            // is the heap property true for all data?
            if (fData.Count == 0) return true;
            int li = fData.Count - 1; // last index
            for (int pi = 0; pi < fData.Count; ++pi) // each parent index
            {
                int lci = 2 * pi + 1; // left child index
                int rci = 2 * pi + 2; // right child index

                if (lci <= li && fData[pi].CompareTo(fData[lci]) > 0) return false; // if lc exists and it's greater than parent then bad.
                if (rci <= li && fData[pi].CompareTo(fData[rci]) > 0) return false; // check the right child too.
            }
            return true; // passed all checks
        } // IsConsistent
    } // PriorityQueue
}
