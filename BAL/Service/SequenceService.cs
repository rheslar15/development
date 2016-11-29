using DAL.Repository;
using DAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Utility;
using Model;
using SQLite;
using System.Diagnostics;


namespace BAL.Service
{
    class SequenceService
    {
        IRepository<SequencesDO> sequencenRepository;
		public SequenceService(SQLiteConnection conn)
        {
			sequencenRepository = RepositoryFactory<SequencesDO>.GetRepository(conn);            
        }

        public List<Sequence> GetSequences()
        {
            List<Sequence> sequences = new List<Sequence>();
			try
            {
                IEnumerable<SequencesDO> sequencesDos = sequencenRepository.GetEntities();
                foreach (SequencesDO seqsDo in sequencesDos)
                {
                    sequences.Add(Converter.GetSequence(sequencenRepository.GetEntity(seqsDo.ID)));
                }
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetSequences method due to " + ex.Message);
			}
            return sequences;
        }

        public Sequence GetSequence(int sequenceID)
        {
            Sequence sequence = new Sequence();
			try
            {
                SequencesDO sequencesDO = sequencenRepository.GetEntity(sequenceID);
                if (sequencesDO != null)
				    sequence = Converter.GetSequence(sequencesDO);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetSequence method due to " + ex.Message);
			}
            return sequence;
        }

        public int SaveSequences(Sequence sequence)
        {
			int result = 0;
			try
            {
                SequencesDO sequencesDO = Converter.GetSequenceDO(sequence);
                 result = sequencenRepository.SaveEntity(sequencesDO);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in SaveSequences method due to " + ex.Message);
			}
            return result;
        }

        public int DeleteInspection(Sequence sequence)
        {
			int result = 0;
			try
            {
                SequencesDO sequencesDO = Converter.GetSequenceDO(sequence);
                 result = sequencenRepository.DeleteEntity(sequencesDO.ID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in DeleteInspection method due to " + ex.Message);
			}
            return result;
        }

    }
}
