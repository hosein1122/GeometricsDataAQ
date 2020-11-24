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

		public virtual void addContinuation(IList<ControlRecord> nextRecordList)
		{
			foreach (ControlRecord cr in nextRecordList)
			{
				addContinuation(cr);
			}
		}

		public virtual void addContinuation(ControlRecord nextRecord)
		{
			subRecords.Add(nextRecord);
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: @Override public void addBlockette(Blockette b) throws SeedFormatException
		public override void addBlockette(Blockette b)
		{
			subRecords[subRecords.Count - 1].addBlockette(b);
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
								prior = PartialBlockette.combine(prior, (PartialBlockette)b);
								if (prior.BytesRead == prior.TotalSize)
								{
									// must have finished last section of partial blockette
									// turn into real and add
									try
									{
										@out.Add(SeedRecord.BlocketteFactory.parseBlockette(prior.Type, prior.ToBytes(), true));
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


		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: @Override public int getNumBlockettes(int type) throws SeedFormatException
		public override int getNumBlockettes(int type)
		{
			return getBlockettes(type).Length;
		}

		public override Blockette[] getBlockettes(int type)
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

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: @Override public void writeASCII(java.io.PrintWriter out, String indent) throws java.io.IOException
		public override void writeASCII(TextWriter @out, string indent)
		{
			@out.Write(indent + "ContinuedControlRecord");
			ControlHeader.writeASCII(@out, indent + "  ");
			foreach (ControlRecord cr in subRecords)
			{
				cr.writeASCII(@out, indent + "    ");
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
