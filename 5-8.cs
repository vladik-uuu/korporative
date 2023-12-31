using System;
using System.Collections.Generic;
using System.Linq;
using Program.Models.Tables;

namespace Program.Tasks
{
    internal class Task_5_8 {
        public static void Run(string[] args){

            #region get data                 
                var ARRAY_B = TableReader.AccessTable<B>("table_B");
                var ARRAY_C = TableReader.AccessTable<C>("table_C");
                var ARRAY_D = TableReader.AccessTable<D>("table_D");                
                var ARRAY_E = TableReader.AccessTable<E>("table_E");
            #endregion get data 

            #region process data 
                var _output = 

                #region make data
                    ARRAY_B.GroupBy(
                        _itemB => _itemB.manufacture_country
                    ).SelectMany(
                        _groupB_by_manufacture_country => ARRAY_E.GroupBy(
                            _itemE => _itemE.consumer_key
                        ).Select(
                            _groupE_by_consumer_key => new {
                                manufacture_country=_groupB_by_manufacture_country.FirstOrDefault(new B()).manufacture_country,
                                
                                consumer_key=_groupE_by_consumer_key.FirstOrDefault(new E()).consumer_key,
                                
                                sum_cost=_groupE_by_consumer_key.Where(
                                        _itemE => _itemE.consumer_key == _groupE_by_consumer_key.FirstOrDefault(new E()).consumer_key
                                    ).Sum(
                                        _itemE => Math.Round(
                                            ARRAY_D.FirstOrDefault(
                                                    _itemD => (_itemD.vendor_code == _itemE.vendor_code) 
                                                            && (_itemD.shop_name == _itemE.shop_name),
                                                    new D()
                                                ).cost
                                            * 
                                            ARRAY_C.FirstOrDefault(
                                                    _itemC => (_itemC.consumer_key == _itemE.consumer_key) 
                                                            && (_itemC.shop_name == _itemE.shop_name),
                                                    new C()
                                                ).discount
                                            )
                                    )
                            }
                        )
                #endregion make data

                #region sort data
                    ).OrderBy( // Sort
                        _item => _item.manufacture_country
                    ).ThenBy(
                        _item => _item.consumer_key
                    ).ThenBy(
                        _item => _item.sum_cost
                #endregion sort data
                
                #region format output
                    ).Select( // format
                        _item => $"{_item.manufacture_country
                                }\t{_item.consumer_key
                                }\t{_item.sum_cost
                                }"
                    );
                #endregion format output
            #endregion process data 

            #region output
                foreach(var _value in _output)
                    Console.WriteLine(_value);
            #endregion output

        }
    }
}
