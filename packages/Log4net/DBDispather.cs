using DGT.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DGT.DBOperation
{
    public class DBDispacther
    {

        private static object syncObject = new object();

        private static List<string> constrList = new List<string>();

        public static bool InsertDataToDB<T>(DispatchData<T> dispathdatas)
        {

            if (dispathdatas.ISTruncateTempDB == true)
            {
                DBOpertor.ExcuteSQlCmd("truncate table " + dispathdatas.DestinationtableName, dispathdatas.ConnectString);
            }
            try
            {
                if (dispathdatas.Datas.Count == 0 || dispathdatas.Datas == null)
                {
                    return true;
                }
                DataTable table = DataTranslateHelper.GennerateTable(dispathdatas.Datas);
                return DBOpertor.BulkInserttoDB(table, dispathdatas.DestinationtableName, dispathdatas.ConnectString);
            }
            catch
            {
                return true;
            }

        }

        public static void DealwithQueueDatas<T>(DispatchData<T> SycDatas)
        {
            DispatchData<T> DispatchDatas = SycDatas;
            string featureconstr = DispatchDatas.ConnectString.GetHashCode().ToString() + "|"
                   + DispatchDatas.DestinationtableName.GetHashCode().ToString();

            lock (syncObject)
            {
                if (constrList.Contains(featureconstr))
                {
                    ThreadPool.QueueUserWorkItem(StoreDatas<T>, SycDatas);
                }
                else
                {
                    constrList.Add(featureconstr);
                    ThreadPool.QueueUserWorkItem(DealDatas<T>, SycDatas);
                }
            }





        }

        private static void StoreDatas<T>(object datas)
        {
            DispatchData<T> DispatchDatas = datas as DispatchData<T>;
            string featureconstr = DispatchDatas.ConnectString.GetHashCode().ToString() + "|"
                   + DispatchDatas.DestinationtableName.GetHashCode().ToString();
            while (true)
            {
                lock (syncObject)
                {
                    if (constrList.Contains(featureconstr))
                    {

                    }
                    else
                    {
                        ThreadPool.QueueUserWorkItem(DealDatas<T>, datas);
                        break;
                    }
                }
                Thread.Sleep(2000);
            }


        }

        private static void DealDatas<T>(object datas)
        {


            DispatchData<T> dispatchdatas = datas as DispatchData<T>;

            string featureconstr = dispatchdatas.ConnectString.GetHashCode().ToString() + "|"
              + dispatchdatas.DestinationtableName.GetHashCode().ToString();

            string connectString = dispatchdatas.ConnectString;
            string destinationtableName = dispatchdatas.DestinationtableName;
            string eventWaitHandleName = "";
            if (dispatchdatas.EventhandlerName == null)
            {
                eventWaitHandleName = destinationtableName;
            }
            else
            {
                eventWaitHandleName = dispatchdatas.EventhandlerName;
            }


            bool issucess = InsertDataToDB<T>(dispatchdatas);
            if (issucess)
            {

                if (dispatchdatas.IsWantToMerged == true)
                {
                    DBOpertor.ExcuteSQlCmd("exec " + dispatchdatas.MergeSQlSPName, dispatchdatas.ConnectString);
                }
                lock (syncObject)
                {
                    constrList.Remove(featureconstr);
                }
            }
            else
            {
                throw new Exception(string.Format("insert DBdata has a error ,in {0}", dispatchdatas.DestinationtableName));
            }


            Console.WriteLine("deal data with " + destinationtableName);

            EventWaitHandle eventWaitHandle;


            if (EventWaitHandle.TryOpenExisting(eventWaitHandleName, out eventWaitHandle))
            {
                if (eventWaitHandle != null)
                {
                    eventWaitHandle.Set();
                }
            }

            //EventWaitHandle eventWaitHandle;
            //try
            //{
            //    eventWaitHandle = EventWaitHandle.OpenExisting(eventWaitHandleName);
            //}
            //catch (Exception e)
            //{
            //    eventWaitHandle = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, eventWaitHandleName);
            //    eventWaitHandle.Reset();
            //}
        }

    }
}
