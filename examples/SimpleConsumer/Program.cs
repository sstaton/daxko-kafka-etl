// Copyright 2016-2017 Confluent Inc., 2015-2016 Andreas Heider
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Derived from: rdkafka-dotnet, licensed under the 2-clause BSD License.
//
// Refer to LICENSE for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Confluent.Kafka.Serialization;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace Confluent.Kafka.Examples.SimpleConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //string brokerList = args[0];
            //var topics = args.Skip(1).ToList();

            //var config = new Dictionary<string, object>
            //{
            //    { "group.id", "simple-csharp-consumer" },
            //    { "bootstrap.servers", brokerList }
            //};

            var config = new Dictionary<string, object>
            {
                { "group.id", "simple-csharp-consumer" },
                { "bootstrap.servers", Environment.GetEnvironmentVariable("brokers") }
            };

            using (var consumer = new Consumer<Null, string>(config, null, new StringDeserializer(Encoding.UTF8)))
            {
                //consumer.Assign(new List<TopicPartitionOffset> { new TopicPartitionOffset(topics.First(), 0, 0) });
                consumer.Assign(new List<TopicPartitionOffset> { new TopicPartitionOffset(Environment.GetEnvironmentVariable("topic"), Convert.ToInt32(Environment.GetEnvironmentVariable("partition")), Convert.ToInt32(Environment.GetEnvironmentVariable("offset"))) });
                
                //string jsonStuff = "{\"Id\":\"59cd626e41a567d0fcb7b550\",\"Timestamp\":\"2017-09-28T15:58:22.608862-05:00\",\"Content\":{\"Id\":\"a953d9da-fa9a-4034-9b01-1ce154003c2b\",\"Member\":{\"Id\":\"effdcdb4-7ebd-494c-bd74-606905a31547\",\"FirstName\":\"Jason\",\"LastName\":\"Little\"},\"Location\":{\"Id\":\"511006ca-f32e-438a-9034-cced82c0599b\",\"LocationName\":\"Ward, Leannon and Mills\"},\"CheckinCompleted\":\"2017-07-02T03:04:21.408729\"},\"CallbackName\":null}";

                //JObject jo = JObject.Parse(jsonStuff);
                //Message obj = JsonConvert.DeserializeObject<Message>(jsonStuff);

                //LocationCheckinData loc = JsonConvert.DeserializeObject<LocationCheckinData>(obj.Content);

                while (true)
                {
                    //TODO Shaun pull from multiple queues, assemble into single insert to combined table
                    Message<Null, string> msg;
                    if (consumer.Consume(out msg, TimeSpan.FromSeconds(1)))
                    {
                        Console.WriteLine($"Topic: {msg.Topic} Partition: {msg.Partition} Offset: {msg.Offset} {msg.Value}");
                        Message obj = JsonConvert.DeserializeObject<Message>(msg.Value);

                        //LocationCheckinData lcd = new LocationCheckinData(new Guid("AE45B6FA-E59B-422E-9E2D-87F25CA38820"),new Guid("AE45B6FA-E59B-422E-9E2D-87F25CA38820"),new Guid("AE45B6FA-E59B-422E-9E2D-87F25CA38820"), Convert.ToDateTime("9/28/2017"), "Shaunn", "Statonn", "HomeBranchh");
                        //Console.WriteLine(lcd.CheckinCompleted.ToString());

                        SqlConnection con = new SqlConnection(Environment.GetEnvironmentVariable("conn"));

                        con.Open();

                        SqlCommand cmd = new SqlCommand("dbo.CheckinMessageToInsertStmts", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("chk_id", obj.Content.Id));
                        cmd.Parameters.Add(new SqlParameter("mem_id", obj.Content.Member.Id));
                        cmd.Parameters.Add(new SqlParameter("loc_id", obj.Content.Location.Id));
                        cmd.Parameters.Add(new SqlParameter("chk_completed", obj.Content.CheckinCompleted));
                        cmd.Parameters.Add(new SqlParameter("first_name", obj.Content.Member.FirstName));
                        cmd.Parameters.Add(new SqlParameter("last_name", obj.Content.Member.LastName));
                        cmd.Parameters.Add(new SqlParameter("loc_name", obj.Content.Location.LocationName));

                        int num = cmd.ExecuteNonQuery();
                        Console.WriteLine("return value: " + num);

                        con.Close();

                        //var jsonData = JObject.Parse(msg.Value);
                        //foreach (var item in jsonData)
                        //{
                        //    Console.WriteLine("item.Key: " + item.Key);
                        //    Console.WriteLine("item.Value: " + item.Value);
                        //    cmd.Parameters.Add(item.Value);
                        //}

                        //using (var reader = cmd.ExecuteReader())
                        //{
                        //    while (reader.Read()) // processes by row
                        //    {
                        //        Console.WriteLine(reader.GetInt32(0));
                        //    }
                        //}

                        //SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[fact_location_member_checkin]", con);
                        //SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[fact_location_member_checkin] ([location_checkin_id],[location_id],[member_id],[checkin_completed],[location_name],[member_first_name],[member_last_name]) VALUES (1, 1, 1, '09/27/2017', 'HomeBranch', 'Shaun', 'Staton')", con);

                        ////TODO Shaun dynamic insert to combined table
                        //int num = cmd.ExecuteNonQuery();
                        //Console.WriteLine("num inserts: " + num);

                        //using (var reader = cmd.ExecuteReader())
                        //{
                        //    while (reader.Read()) // processes by row
                        //    {
                        //        Console.WriteLine(reader.GetInt32(0));
                        //    }
                        //}
                    }
                }
            }
        }
    }
}
