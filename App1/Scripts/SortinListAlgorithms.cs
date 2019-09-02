using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Scripts
{
    public class SortinListAlgorithms
    {
        public class SortList : IComparer<Block>
        {
            public virtual int Compare(Block x, Block y)
            {
                throw new NotImplementedException();
            }
        }
        public class SortListByName : SortList
        {
            public override int Compare(Block x, Block y)
            {
                return string.Compare(x.Link, y.Link);
            }
        }
        public class SortListByTime : SortList
        {
            public override int Compare(Block x, Block y)
            {
                if (x.TimeTaken > y.TimeTaken)
                {
                    return 1;
                }
                if (x.TimeTaken < y.TimeTaken)
                {
                    return -1;
                }

                return 0;
            }
        }

        public class SortListByAvailability : SortList
        {
            public override int Compare(Block x, Block y)
            {
                if (x.IsAvailable && !y.IsAvailable)
                {
                    return 1;
                }
                if (!x.IsAvailable && y.IsAvailable)
                {
                    return -1;
                }

                return 0;
            }
        }

    }
}
