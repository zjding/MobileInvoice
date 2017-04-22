using System;
using Foundation;

using CloudKit;
using System.Threading.Tasks;

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
				await privateDatabase.SaveRecordAsync(record);
			}
			catch (Exception e)
			{
				Console.WriteLine("An error occured: {0}", e.Message);
			}
		}
	}
}
