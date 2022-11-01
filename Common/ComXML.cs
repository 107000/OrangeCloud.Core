using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OrangeCloud.Core.Common
{
    public class ComXML
    {
        /// <summary>
        /// 根据XML字符串获取DataSet
        /// </summary>
        /// <param name="strXML">XML字符串</param>
        /// <returns>DataSet</returns>
        public static DataSet StrXMLToDataSet(string strXML)
        {
            DataSet dsModel = new DataSet();

            System.IO.StringReader xr = new System.IO.StringReader(strXML);

            dsModel.ReadXml(xr);

            return dsModel;
        }

        //----------------------------------------------------------------------------
        //<?xml version="1.0" encoding="utf-8" ?>
        //<userdata>
        //    <dataconnection>
        //        <server>localhost</server>
        //        <uid>sa</uid>
        //        <pwd>111</pwd>
        //    </dataconnection>
        //    <dataconnection>
        //        <server>localhost2</server>
        //        <uid>sa2</uid>
        //        <pwd>222</pwd>
        //    </dataconnection>
        //</userdata>
        //得到：
        //server uid pwd 
        //localhost sa 111 
        //localhost2 sa2 222 

        /// <summary>
        /// 根据XML文件获取DataSet
        /// </summary>
        /// <param name="XMLPath">XML文件所在路径：Server.MapPath("XMLFile.xml")</param>
        /// <returns>DataSet</returns>
        public static DataSet XMLToDataSet(string XMLPath)
        {
            DataSet dsModel = new DataSet();

            dsModel.ReadXml(XMLPath);

            return dsModel;
        }
    }
}
