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


namespace Confluent.Kafka.Examples.SimpleConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string brokerList = args[0];
            var topics = args.Skip(1).ToList();

            var config = new Dictionary<string, object>
            {
                { "group.id", "simple-csharp-consumer" },
                { "bootstrap.servers", brokerList }
            };

            using (var consumer = new Consumer<Null, string>(config, null, new StringDeserializer(Encoding.UTF8)))
            {
                consumer.Assign(new List<TopicPartitionOffset> { new TopicPartitionOffset(topics.First(), 0, 0) });

                //SqlConnection con = new SqlConnection("Data source = [ip]; Initial catalog = [db name]; User id = [user name]; password = [password]");
                SqlConnection con = new SqlConnection("Data Source=.\\playsqlexpress;Initial Catalog=kafka-dw;Integrated Security=true;");

                while (true)
                {
                    Message<Null, string> msg;
                    if (consumer.Consume(out msg, TimeSpan.FromSeconds(1)))
                    {
                        Console.WriteLine($"Topic: {msg.Topic} Partition: {msg.Partition} Offset: {msg.Offset} {msg.Value}");
                        con.Open();
                        //SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[fact_location_member_checkin]", con);
                        SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[fact_location_member_checkin]            ([location_checkin_id]            ,[location_id]            ,[member_id]           ,[checkin_completed]           ,[location_name]           ,[member_first_name]           ,[member_last_name])     VALUES           (1           , 1           , 1           , '09/27/2017'           , 'HomeBranch'           , 'Shaun'           , 'Staton')", con);

                        int num = cmd.ExecuteNonQuery();
                        Console.WriteLine("num inserts: " + num);

                        //using (var reader = cmd.ExecuteReader())
                        //{
                        //    while (reader.Read()) // processes by row
                        //    {
                        //        Console.WriteLine(reader.GetInt32(0));
                        //    }
                        //}
                        con.Close();
                    }
                }
            }
        }
    }
}
