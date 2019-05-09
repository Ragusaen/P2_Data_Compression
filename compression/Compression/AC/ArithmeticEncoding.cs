using System.Collections.Generic;
using Compression.AC;

/*
 * Arithmetic encoding class
 * Contains all methods for calculating unique tag
 */
namespace Compression.Arithmetic {
    public class ArithmeticEncoding : DataFileIterator {

        public ArithmeticEncoding(DataFile file) : base(file) {
            this.file = file; 
        }
        
        
        #region Methods
        /*
         * Contains Encoding methods 
         */

        #region Encoding file method

        public DataFile EncodeFIle() {
            return null;
        }

        #endregion
       
        #endregion
    }
}