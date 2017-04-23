using System;
using Foundation;

using CloudKit;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MobileInvoice.ios
{
	public class CloudManager : NSObject
	{
		readonly CKContainer container;
		CKDatabase privateDatabase;
		CKDatabase publicDatabase;

		public CloudManager()
		{
			container = CKContainer.DefaultContainer;
			privateDatabase = container.PrivateCloudDatabase;
			publicDatabase = container.PublicCloudDatabase;
		}

		public async Task SaveAsync(CKRecord record)
		{
			try
			{
				await publicDatabase.SaveRecordAsync(record);
			}
			catch (Exception e)
			{
				Console.WriteLine("An error occured: {0}", e.Message);
			}
		}

		public void FetchRecords(string recordType, Action<List<CKRecord>> completionHandler)
		{
			var truePredicate = NSPredicate.FromValue(true);
			var query = new CKQuery(recordType, truePredicate)
			{
				SortDescriptors = new[] { new NSSortDescriptor("creationDate", false) }
			};

			var queryOperation = new CKQueryOperation(query)
			{
				DesiredKeys = new[] { "value" }
			};

			var results = new List<CKRecord>();

			queryOperation.RecordFetched = (record) =>
			{
				results.Add(record);
			};

			queryOperation.Completed = (cursor, error) =>
			{
				if (error != null)
				{
					Console.WriteLine("An error occured: {0}", error.Description);
					return;
				}

				InvokeOnMainThread(() => completionHandler(results));
			};

			privateDatabase.AddOperation(queryOperation);
		}

		async public Task<List<CKRecord>> FetchRecordsByTypeAndPredicate(string recordType, NSPredicate predicate)
		{
			List<CKRecord> records = new List<CKRecord>();

			CKQuery query = new CKQuery(recordType, predicate);

			CKRecord[] results = await publicDatabase.PerformQueryAsync(query, CKRecordZone.DefaultRecordZone().ZoneId);

			records.AddRange(results);

			return records;
		}
	}
}
