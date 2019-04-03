using System.Collections.Generic;
using System.Linq;

namespace Compression.PPM{
    public class ContextTable{
        public List<ContextTableContent> ContextList; 
        public int TotalCount { get; set; }

        public void UpdateContext(byte[] context, byte[] a) {
            int i = ContextAlreadyExist(a);
            
            if (i >= 0) {
                ContextList[i] = null;
            }
            else {
                // ContextList.Append(new ContextTableContent());
            }
        }
        
        private int ContextAlreadyExist(byte[] a) {
            ByteArrayComparer byteArrayComparer = new ByteArrayComparer();
            
            for (int i = 0; i < ContextList.Count; i++) {
                if(byteArrayComparer.Compare(ContextList[i].Context, a) == 0)
                    return i; 
            }

            return -1;
        }
    }
}