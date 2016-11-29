using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface ITraversible
    {
        string getName();
        bool prevSeqNextClicked { get; set; }
        bool enableRow { get; set; }
        int getSequenceID();
    }
}