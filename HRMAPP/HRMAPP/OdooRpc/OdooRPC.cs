﻿using HRMAPP.Model;
using HRMAPP.OdooRpc.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace HRMAPP.OdooRpc
{
    class OdooRPC
    {

        private string baseUrl;
        private string jsonRpcUrl;

        public string Database { get; set; }
        public string Username { get; set; }
        public int Uid { get; set; }
        public string Password { get; set; }

        private OdooRPC(string url)
        {
            this.baseUrl = url;
            this.jsonRpcUrl = this.baseUrl + "/jsonrpc";
        }

        public static OdooRPC InstanceCreation(string url)
        {
            return new OdooRPC(url);
        }

        public int login(String database, String username, String password)
        {
            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("common", "login", new object[] { database, username, password });

            //JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("common", "login", new object[] { Settings.UserDbName, Settings.UserName, Settings.UserPassword });
            try
            {

                this.Database = database;
                this.Username = username;
                this.Password = password;
                var tempId = odooServerCall<dynamic>(jsonRpcUrl, parameters);
                int userId = (int)tempId;
                this.Uid = userId;

                if (userId == 0)
                {
                    if (App.userid == 0)
                    {
                        userId = App.userid_db;

                    }

                    else
                    {
                        userId = App.userid;
                    }

                }
                return userId;

            }
            catch (Exception ea)
            {
                throw new Exception("Odoo error!!" + ea.Message);
            }

        }


        public T odooSearchCall<T>(string model, object[] domain, bool innerArray)
        {
            JsonRpcRequestParameter parameters;
            if (innerArray)
            {
                parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "search", domain });
            }
            else
            {
                parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "search", new object[] { domain } });
            }

            T responseData = odooServerCall<T>(jsonRpcUrl, parameters);
            return responseData;
        }

        public T odooSearchReadCall<T>(string model, object[] domain, string[] fields, bool innerArray)
        {
            JsonRpcRequestParameter parameters;
            if (innerArray)
            {

                parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "search_read", domain, fields });
            }
            else if (domain.Length == 0)
            {
                parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "search_read", new object[] { }, fields });
            }

            else
            {
                parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "search_read", new object[] { domain }, fields });
            }

            T searchData = odooServerCall<T>(jsonRpcUrl, parameters);
            return searchData;
        }

        public dynamic odooMethodCall_cal<T>(string model, string method, int recId, int month, int year)
        {
            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, recId, month, year });
            T responseData = odooServerCall<T>(jsonRpcUrl, parameters);
            try
            {
                if (responseData.ToString().Equals("Odoo Error"))
                {
                    return responseData;
                }
            }
            catch { }
            return responseData;
        }



        //public List<StockWareHouseList> odooMethodCall_stockcount<T>(string model, string method, int locid)
        //{
        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { }, locid });
        //    JArray stockData = odooServerCall<JArray>(jsonRpcUrl, parameters);

        //    List<StockWareHouseList> result = stockData.ToObject<List<StockWareHouseList>>();

        //    return result;
        //}


        //public List<StockWareHouseList> odooMethodCall_allcountdata<T>(string model, string method, int prod_id, int loc_id)
        //{
        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { }, prod_id, loc_id });
        //    JArray stockData = odooServerCall<JArray>(jsonRpcUrl, parameters);

        //    List<StockWareHouseList> result = stockData.ToObject<List<StockWareHouseList>>();

        //    return result;
        //}

        public JObject odooFilterDataCall(string model, string method)
        {
            if (this.Uid == 0)
            {
                this.Uid = App.userid_db;
            }

            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, Settings.UserPassword, model, method, new object[] { }, App.filterdict });
            JObject responseData = odooServerCall<JObject>(jsonRpcUrl, parameters);
            return responseData;
        }

        public dynamic odooMethodCall_meeting<T>(string model, string method, int userid)
        {
            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { }, userid });
            T responseData = odooServerCall<T>(jsonRpcUrl, parameters);

            try
            {
                if (responseData.ToString().Equals("Odoo Error"))
                {
                    return responseData;
                }
            }
            catch { }
            return responseData;
        }

        public dynamic odooMethodCall<T>(string model, string method, int recId)
        {
            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, recId });
            T responseData = odooServerCall<T>(jsonRpcUrl, parameters);
            try
            {
                if (responseData.ToString().Equals("Odoo Error"))
                {
                    return responseData;
                }
            }
            catch { }
            return responseData;
        }

        //public List<CustomersModel> odooSearchReadCall3<T>(string model, object[] domain, string[] fields, bool innerArray)
        //{
        //    JsonRpcRequestParameter parameters;
        //    if (innerArray)
        //    {

        //        parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "search_read", domain, fields });
        //    }
        //    else if (domain.Length == 0)
        //    {
        //        parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "search_read", new object[] { }, fields });
        //    }

        //    else
        //    {
        //        parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "search_read", new object[] { domain }, fields });
        //    }

        //    JArray searchData = odooServerCall<JArray>(jsonRpcUrl, parameters);

        //    List<CustomersModel> result = searchData.ToObject<List<CustomersModel>>();
        //    return result;
        //}

        //public List<SalesModel> odooSearchReadCall1<T>(string model, object[] domain, string[] fields, bool innerArray)
        //{
        //    JsonRpcRequestParameter parameters;
        //    if (innerArray)
        //    {
        //        parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "search_read", domain, fields });
        //    }
        //    else if (domain.Length == 0)
        //    {
        //        parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "search_read", new object[] { }, fields });
        //    }

        //    else
        //    {
        //        parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "search_read", new object[] { domain }, fields });
        //    }

        //    //  List<TestModel> searchData = odooServerCall<JArray>(jsonRpcUrl, parameters) as List<TestModel>;
        //    JArray searchData = odooServerCall<JArray>(jsonRpcUrl, parameters);

        //    List<SalesModel> result = searchData.ToObject<List<SalesModel>>();
        //    return result;
        //}


        //public bool odooUpdatecrmOppMeeting(string model, string method, Dictionary<string, dynamic> vals)
        //{
        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] {  }, vals });
        //    bool result = odooServerCall<bool>(jsonRpcUrl, parameters);
        //    return result;
        //}


        //public string odooUpdatecrmOppMeeting(string model, string method, Dictionary<string, dynamic> vals)
        //{
        //    if (this.Uid == 0)
        //    {
        //        this.Uid = App.userid_db;
        //    }

        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { }, vals });
        //    string result = odooServerCall<string>(jsonRpcUrl, parameters);
        //    return result;
        //}


        //public bool odooUpdatesaleorder(string model, string method, int sale_id, Dictionary<string, dynamic> vals)
        //{
        //    if (this.Uid == 0)
        //    {
        //        this.Uid = App.userid_db;
        //    }

        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { sale_id }, vals });
        //    var result = odooServerCall<bool>(jsonRpcUrl, parameters);
        //    return result;
        //}

        //public bool odooUpdatecustomer(string model, string method, int cus_id, Dictionary<string, dynamic> vals)
        //{
        //    if (this.Uid == 0)
        //    {
        //        this.Uid = App.userid_db;
        //    }

        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { cus_id }, vals });
        //    var result = odooServerCall<bool>(jsonRpcUrl, parameters);
        //    return result;
        //}

        public bool odoocreateAttendances(string model, string method, Dictionary<string, dynamic> vals)
        {
            if (this.Uid == 0)
            {
                this.Uid = App.userid_db;
            }

            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method,new object[] {}, vals });
            bool result = odooServerCall<bool>(jsonRpcUrl, parameters);
            //if (result > 0)
            //{
            //   // App.atten_checkIn_id = result;
            //  //  Settings.CheckIn_ID = App.atten_checkIn_id.ToString();
            //    result = true;
            //}
            //else
            //{
            //    result = false;
            //}
            return result;
        }
                      

        public bool odooUpdateAttendances(string model, string method, int checkin_id, Dictionary<string, dynamic> vals)
        {
            if (this.Uid == 0)
            {
                this.Uid = App.userid_db;
            }

            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { checkin_id }, vals });
            var result = odooServerCall<bool>(jsonRpcUrl, parameters);
            return result;
        }

       
        public string odoocreateLeaves(string model, string method, Dictionary<string, dynamic> vals)
        {
            if (this.Uid == 0)
            {
                this.Uid = App.userid_db;
            }

            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, Settings.UserPassword, model, method, new object[] { }, vals });
            string result = odooServerCall<string>(jsonRpcUrl, parameters);
            return result;
        }

        public string odooupdateLeaves(string model, string method,int id,Dictionary<string, dynamic> vals)
        {
            if (this.Uid == 0)
            {
                this.Uid = App.userid_db;
            }

            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, Settings.UserPassword, model, method, new object[] {id}, vals });
            string result = odooServerCall<string>(jsonRpcUrl, parameters);
            return result;
        }

        public bool odoodeleteattachment(string model, string method, int att_id)
        {
            if (this.Uid == 0)
            {
                this.Uid = App.userid_db;
            }

            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { }, att_id });
            var result = odooServerCall<bool>(jsonRpcUrl, parameters);
            return result;
        }


        public bool odoosubmittomanager(string model, string method, int att_id)
        {
            if (this.Uid == 0)
            {
                this.Uid = App.userid_db;
            }

            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] {att_id}  });
            var result = odooServerCall<bool>(jsonRpcUrl, parameters);
            return result;
        }

        public bool odootimesheetsubmit(string model, string method, Dictionary<string, dynamic> vals)
        {
            if (this.Uid == 0)
            {
                this.Uid = App.userid_db;
            }

            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] {}, vals  });
            var result = odooServerCall<bool>(jsonRpcUrl, parameters);
            return result;
        }


        //public string odooUpdateGpsData(string model, string method, float lat, float longitude, int id)
        //{
        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, lat, longitude, id });
        //    string result = odooServerCall<string>(jsonRpcUrl, parameters);
        //    return result;
        //}

        //public string odooUpdateGpsData1(string model, string method, float lat, float longitude)
        //{
        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { App.userid }, lat, longitude });
        //    var result = odooServerCall<bool>(jsonRpcUrl, parameters);
        //    return result;
        //}


        //public string clearGpsData(string model, string method, int id)
        //{
        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { id } });
        //    string result = odooServerCall<string>(jsonRpcUrl, parameters);
        //    return result;
        //}

        //public string odooUpdatecrmLeadCreation(string model, string method, Dictionary<string, dynamic> vals)
        //{
        //    if (this.Uid == 0)
        //    {
        //        this.Uid = App.userid_db;
        //    }

        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, vals });
        //    string result = odooServerCall<string>(jsonRpcUrl, parameters);
        //    return result;
        //}

        //public string odooConvertrmLeadCreation(string model, string method, int id, Dictionary<string, dynamic> vals)
        //{
        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, id, vals });
        //    string result = odooServerCall<string>(jsonRpcUrl, parameters);
        //    return result;
        //}

        //public string odooUpdatecrmOppMeeting1(string model, string method, int updateId, Dictionary<string, dynamic> vals)
        //{
        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { updateId }, vals });
        //    string result = odooServerCall<string>(jsonRpcUrl, parameters);
        //    return result;
        //}

        //public bool odooUpdateUserData(string model, string method, int modelId)
        //{

        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { modelId } });
        //    var responseData = odooServerCall<bool>(jsonRpcUrl, parameters);
        //    return responseData;
        //}

        //public bool odooLostData(string model, string method, int modelId, int lostId)
        //{

        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { }, modelId, lostId });
        //    var responseData = odooServerCall<bool>(jsonRpcUrl, parameters);
        //    return responseData;
        //}

        //public JObject odooCrmLeadDataCall(string model, string method)
        //{

        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { } });
        //    JObject responseData = odooServerCall<JObject>(jsonRpcUrl, parameters);
        //    //List<CRMModel> result = responseData.ToObject<List<CRMModel>>();
        //    return responseData;
        //}

        //public JObject odooFilterDataCall(string model, string method)
        //{
        //    if (this.Uid == 0)
        //    {
        //        this.Uid = App.userid_db;
        //    }

        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { }, App.filterdict });
        //    JObject responseData = odooServerCall<JObject>(jsonRpcUrl, parameters);
        //    //List<CRMModel> result = responseData.ToObject<List<CRMModel>>();
        //    return responseData;
        //}

        //public List<CRMModel> odooSearchReadCall2<T>(string model, object[] domain, string[] fields, bool innerArray)
        //{
        //    JsonRpcRequestParameter parameters;

        //    if (innerArray)
        //    {
        //        parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "search_read", domain, fields });
        //    }
        //    else if (domain.Length == 0)
        //    {
        //        parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "search_read", new object[] { }, fields });
        //    }

        //    else
        //    {
        //        parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "search_read", new object[] { domain }, fields });
        //    }

        //    JArray searchData = odooServerCall<JArray>(jsonRpcUrl, parameters);

        //    List<CRMModel> result = searchData.ToObject<List<CRMModel>>();

        //    return result;
        //}

        public Int32 odooCreateCall(string model, Dictionary<string, dynamic> vals)
        {
            if (vals.Count > 0)
            {
                try
                {
                    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "create", vals });
                    Int32 responseData = odooServerCall<Int32>(jsonRpcUrl, parameters);
                    return responseData;
                }
                catch
                {
                    throw;
                }
            }
            return -1;
        }

        public bool odooWriteCall(string model, long recId, Dictionary<string, dynamic> vals)
        {
            if (vals.Count > 0)
            {
                JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, "write", recId, vals });
                var responseData = odooServerCall<bool>(jsonRpcUrl, parameters);
                return responseData;
            }
            return true;
        }

        public T odooUnlinkCall<T>(string model, int recId)
        {
            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, recId });
            T responseData = odooServerCall<T>(jsonRpcUrl, parameters);
            return responseData;
        }


        public dynamic odooMethodCall<T>(string model, string method, List<int> ids)
        {
            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, ids });
            T responseData = odooServerCall<T>(jsonRpcUrl, parameters);
            try
            {
                if (responseData.ToString().Equals("Odoo Error"))
                {
                    return responseData;
                }
            }
            catch { }
            return responseData;
        }


        public string odooMethodArgsCall(string model, string method, Dictionary<string, dynamic> vals)
        {
            if (vals.Count > 0)
            {
                JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, vals });
                string responseData = odooServerCall<string>(jsonRpcUrl, parameters);
                return responseData;
            }
            return "";
        }



        //public dynamic odooMethodSaleOrderConfirm(string model, string method, int saleorderId)
        //{
        //    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, saleorderId });
        //    string responseData = odooServerCall<string>(jsonRpcUrl, parameters);
        //    // return responseData;
        //    try
        //    {
        //        if (responseData.ToString().Equals("Odoo Error"))
        //        {
        //            return responseData;
        //        }
        //    }
        //    catch { }
        //    return responseData;
        //}

        public dynamic getDatabases()
        {
            try
            {

                if (Device.OS.ToString() == "Android")
                {
                    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("db", "list");
                    string[] rawResult = odooServerCall<string[]>(jsonRpcUrl, parameters);
                    return rawResult;
                }
                else
                {
                    JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("db", "list");
                    var rawResult = odooServerCall<string[]>(jsonRpcUrl, parameters);
                    string[] dbData = rawResult.ToObject<string[]>();
                    return dbData;
                }
            }
            catch (Exception ea)
            {
                System.Diagnostics.Debug.WriteLine(ea.Message);
                throw;
            }
        }

        public dynamic odooMethodArgsCall<T>(string model, string method, List<int> recIds)
        {
            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { }, recIds });
            T responseData = odooServerCall<T>(jsonRpcUrl, parameters);
            try
            {
                if (responseData.ToString().Equals("Odoo Error"))
                {
                    return responseData;
                }
            }
            catch { }
            return responseData;
        }

        public dynamic odooMethodArgsCall<T>(string model, string method, int recId)
        {
            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { }, recId });
            T responseData = odooServerCall<T>(jsonRpcUrl, parameters);
            try
            {
                if (responseData.ToString().Equals("Odoo Error"))
                {
                    return responseData;
                }
            }
            catch { }
            return responseData;
        }

        public dynamic odooMethodArgsCall2<T>(string model, string method, int recId)
        {
            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { recId } });
            T responseData = odooServerCall<T>(jsonRpcUrl, parameters);
            try
            {
                if (responseData.ToString().Equals("Odoo Error"))
                {
                    return responseData;
                }
            }
            catch { }
            return responseData;
        }

        public dynamic odooMethodArgsCall<T>(string model, string method)
        {
            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { } });
            T responseData = odooServerCall<T>(jsonRpcUrl, parameters);
            try
            {
                if (responseData.ToString().Equals("Odoo Error"))
                {
                    return responseData;
                }
            }
            catch { }
            return responseData;
        }

        public dynamic odooAttendanceCall<T>(string model, string method)
        {
            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, this.Password, model, method, new object[] { } });
            T responseData = odooServerCall<T>(jsonRpcUrl, parameters);
            try
            {
                if (responseData.ToString().Equals("Odoo Error"))
                {
                    return responseData;
                }
            }
            catch
            {
                int k = 0;
            }
            return responseData;
        }

        public dynamic odooMethodCall_gettimesheet<T>(string model, string method)
        {
            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, Settings.UserPassword, model, method, new object[] { } });
            T responseData = odooServerCall<T>(jsonRpcUrl, parameters);

            try
            {
                if (responseData.ToString().Equals("Odoo Error"))
                {
                    return responseData;
                }
            }
            catch { }
            return responseData;
        }

        public dynamic odooMethodCall_gettimesheet_details<T>(string model, string method)
        {
            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, Settings.UserPassword, model, method, new object[] {App.userid } });
            T responseData = odooServerCall<T>(jsonRpcUrl, parameters);

            try
            {
                if (responseData.ToString().Equals("Odoo Error"))
                {
                    return responseData;
                }
            }
            catch { }
            return responseData;
        }

        public dynamic odooMethodCall_getattendance_details<T>(string model, string method)
        {
            JsonRpcRequestParameter parameters = new JsonRpcRequestParameter("object", "execute", new object[] { this.Database, this.Uid, Settings.UserPassword, model, method, new object[] {},App.userid });
            T responseData = odooServerCall<T>(jsonRpcUrl, parameters);

            try
            {
                if (responseData.ToString().Equals("Odoo Error"))
                {
                    return responseData;
                }
            }
            catch { }
            return responseData;
        }

        private dynamic odooServerCall<T>(string url, JsonRpcRequestParameter parameter)
        {
            var tmpData = new JsonRpcRequest(parameter);
            string requestContent = "";

            if (Device.RuntimePlatform == "Android")
            {
                requestContent = JsonConvert.SerializeObject(tmpData);
            }
            else
            {
                Dictionary<string, dynamic> paramsDic = new Dictionary<string, dynamic>();
                paramsDic.Add("service", parameter.Service);
                paramsDic.Add("method", parameter.Method);
                paramsDic.Add("args", parameter.Argument);

                Dictionary<string, dynamic> jObj = new Dictionary<string, dynamic>();
                jObj.Add("jsonrpc", "2.0");
                jObj.Add("method", "call");
                jObj.Add("params", paramsDic);

                var formatData = JsonConvert.SerializeObject(jObj);
                requestContent = formatData;
            }
            System.Diagnostics.Debug.WriteLine("WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW " + requestContent);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "POST";
            try
            {
                App.responseState = true;
                App.NetAvailable = true;
                IAsyncResult resultRequest = request.BeginGetRequestStream(null, null);
                Stream streamInput = request.EndGetRequestStream(resultRequest);
                byte[] byteArray = Encoding.UTF8.GetBytes(requestContent);
                streamInput.WriteAsync(byteArray, 0, byteArray.Length);
                streamInput.FlushAsync();

                // Receive data from server
                IAsyncResult resultResponse = request.BeginGetResponse(null, null);
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(resultResponse);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                var resultData = streamRead.ReadToEndAsync();
                var responseData = resultData.Result;
                streamResponse.FlushAsync();
                if (Device.RuntimePlatform == "iOS")
                {
                    string rspData = string.Join("", responseData);
                    try
                    {
                        JObject jsonObj = JObject.Parse(rspData);
                        Dictionary<string, dynamic> dictObj = jsonObj.ToObject<Dictionary<string, dynamic>>();
                        return dictObj["result"];
                    }
                    catch (Exception ea)
                    {
                        return responseData;
                    }

                }
                else
                {
                    try
                    {
                        JsonRpcResponse<T> jsonRpcResponse = JsonConvert.DeserializeObject<JsonRpcResponse<T>>(responseData);
                        
                        //System.Diagnostics.Debug.WriteLine("jjjjjjjjjjjjjjjjjjjjjjjjjjj " + json);

                        if (jsonRpcResponse.Error != null)
                        {                            
                            return "Odoo Error";
                        }
                        else
                        {
                            return jsonRpcResponse.Result;
                        }
                    }
                    catch (Exception ea)
                    {
                        return responseData;
                    }
                }

            }
            catch (Exception ea)
            {
                if (ea.Message.Contains("(Network is unreachable)") || ea.Message.Contains("NameResolutionFailure"))
                {
                    App.NetAvailable = false;
                }

                else if (ea.Message.Contains("(503) Service Unavailable"))
                {
                    App.responseState = false;
                }

                System.Diagnostics.Debug.WriteLine(ea.Message);
                //  throw;
            }
            return default(T);
        }

    }
}
