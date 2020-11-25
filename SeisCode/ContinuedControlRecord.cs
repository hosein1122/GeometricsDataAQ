using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeisCode
{
	public class ContinuedControlRecord : ControlRecord
	{

		public ContinuedControlRecord(ControlRecord first) : base(first.ControlHeader)
		{
			subRecords.Add(first);
		}

		public virtual void AddContinuation(IList<ControlRecord> nextRecordList)
		{
			foreach (ControlRecord cr in nextRecordList)
			{
				AddContinuation(cr);
			}
		}

		public virtual void AddContinuation(ControlRecord nextRecord)
		{
			subRecords.Add(nextRecord);
		}

		public override void AddBlockette(Blockette b)
		{
			subRecords[subRecords.Count - 1].AddBlockette(b);
		}

		public override Blockette[] Blockettes
		{
			get
			{
				PartialBlockette prior = null;
				IList<Blockette> @out = new List<Blockette>();
				foreach (ControlRecord cr in subRecords)
				{
					Blockette[] subB = cr.Blockettes;
					foreach (Blockette b in subB)
					{
						if (b is PartialBlockette)
						{
							if (prior == null)
							{
								prior = (PartialBlockette)b;
							}
							else
							{
								prior = PartialBlockette.Combine(prior, (PartialBlockette)b);
								if (prior.BytesRead == prior.TotalSize)
								{
									// must have finished last section of partial blockette
									// turn into real and add
									try
									{
										@out.Add(SeedRecord.BlocketteFactory.ParseBlockette(prior.Type, prior.ToBytes(), true));
										prior = null;
									}
									catch (Exception e)
									{
										throw new Exception("Unable to combine partial blockettes into a single real bloackette.", e);
									}
								}
							}
						}
						else
						{
							if (prior != null)
							{
								throw new Exception("Found regular blockette waiting for rest of partial blockette: " + prior.BytesRead + " out of " + prior.TotalSize + " bytes.");
							}
							@out.Add(b);
						}
					}
				}
				if (prior != null)
				{
					throw new Exception("Found partial blockette at end, rest of bytes missing: " + prior.BytesRead + " out of " + prior.TotalSize + " bytes.");
				}
				return ((List<Blockette>)@out).ToArray();
			}
		}

			
		public override int GetNumBlockettes(int type)
		{
			return GetBlockettes(type).Length;
		}

		public override Blockette[] GetBlockettes(int type)
		{
			IList<Blockette> @out = new List<Blockette>();
			foreach (ControlRecord cr in subRecords)
			{
				Blockette[] subB = cr.Blockettes;
				foreach (Blockette b in subB)
				{
					if (b.Type == type)
					{
						@out.Add(b);
					}
				}
			}
			return ((List<Blockette>)@out).ToArray();
		}

		public override void WriteASCII(TextWriter @out, string indent)
		{
			@out.Write(indent + "ContinuedControlRecord");
			ControlHeader.WriteASCII(@out, indent + "  ");
			foreach (ControlRecord cr in subRecords)
			{
				cr.WriteASCII(@out, indent + "    ");
			}
		}

		public virtual IList<ControlRecord> SubRecords
		{
			get
			{
				return subRecords;
			}
		}

		internal IList<ControlRecord> subRecords = new List<ControlRecord>();
	}

}
