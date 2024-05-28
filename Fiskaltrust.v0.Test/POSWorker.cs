using Newtonsoft.Json;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IntelliSoft.CashBox.Austria.Domain
{
    public class POSWorker
    {


        internal POSWorker(fiskaltrust.ifPOS.v0.IPOS posProxy)
        {
            myPOSProxy = posProxy;
            myIsInitialied = true;
        }
        public bool IsInitialized => myIsInitialied;

        private bool myIsInitialied { get; init; } = false;
        private fiskaltrust.ifPOS.v0.IPOS myPOSProxy;
        public async Task<Stream?> GetJournalAsync(long journal, long fromDate, long toDate)
        {
            try
            {
                if (IsInitialized == false)
                {
                    throw new Exception("Missing class initialization");
                }

               var locTaskCompletionSource = new TaskCompletionSource<Stream?>();

                await Task.Run(() =>
                {
                    try
                    {

                        var locReturn = myPOSProxy.Journal(journal, fromDate, toDate);

                        locTaskCompletionSource.SetResult(locReturn);

                    }
                    catch (Exception ex)
                    {
                        locTaskCompletionSource.SetException(ex);
                    }
                });
                return await locTaskCompletionSource.Task;
            } catch
            {
                return null;
            }
        }

        public async Task<fiskaltrust.ifPOS.v0.ReceiptResponse?> GetSignedDataAsync(fiskaltrust.ifPOS.v0.ReceiptRequest data)
        {
            try
            {
                if (IsInitialized == false)
                {
                    throw new Exception("Missing class initialization");
                }

                var locTaskCompletionSource = new TaskCompletionSource<fiskaltrust.ifPOS.v0.ReceiptResponse?>();


                await Task.Run(() =>
                {
                    try
                    {

                        var locReturn = myPOSProxy.Sign(data);

                        locTaskCompletionSource.SetResult(locReturn);

                    }
                    catch (Exception ex)
                    {
                        locTaskCompletionSource.SetException(ex);
                    }
                });
                return await locTaskCompletionSource.Task;
                
            } catch
            {
                return null;
            }
        }

        public async Task<bool> IsAvailableAsync()
        {

            string locEchtoTest = "Test the echo";
            try
            {
                if (IsInitialized == false)
                {
                    throw new Exception("Missing class initialization");
                }

                var locTaskCompleteSource = new TaskCompletionSource<string>();

                await Task.Run(() =>
                {
                    try
                    {
                        var locReturn = myPOSProxy.Echo(locEchtoTest);
                        locTaskCompleteSource.SetResult(locReturn);

                    }
                    catch (Exception ex)
                    {
                        locTaskCompleteSource.SetException(ex);
                    }
                });
                var locReturn = await locTaskCompleteSource.Task;

                if (locReturn == locEchtoTest)
                {
                    return true;

                }
                else
                    return false;
            }
            catch
            {
                return false;
            }

                   

        }


        internal static POSWorker Init(fiskaltrust.ifPOS.v0.IPOS posProxy) => new POSWorker(posProxy);


        

    }
}
