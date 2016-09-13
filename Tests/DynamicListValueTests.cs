using System;
using Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class DynamicListValueTests
    {
        private class TestClass : IEquatable<TestClass>
        {
            public string Name => Guid.NewGuid().ToString();

            public bool Equals(TestClass other)
            {
                return Name == other.Name;
            }
        }

        [TestMethod]
        public void DynamicListValue_BasicTest()
        {
            string name = "John";
            string middleName = "Doc";
            string lastName = "Smith";
            string randomName = new Random().Next().ToString();

            DynamicListValue lValue = new DynamicListValue();

            Assert.IsNotNull(lValue);

            lValue.Value = name;
            lValue.Add(lastName);
            lValue.Insert(1, middleName);

            Assert.AreEqual(3, lValue.Count);
            Assert.AreEqual(name, lValue.Value);
            Assert.AreEqual(middleName, lValue[1]);
            Assert.AreEqual(lastName, lValue[2]);

            lValue -= middleName;
            lValue -= name;
            lValue += randomName;

            Assert.AreEqual(2, lValue.Count);
            Assert.AreEqual(lastName, lValue.Value);
            Assert.AreEqual(randomName, lValue[1]);

            lValue.Clear();

            Assert.AreEqual(0, lValue.Count);
            Assert.IsFalse(lValue.HasValue);

            var list = new List<TestClass>
            {
                new TestClass(),
                new TestClass(),
                new TestClass()
            };

            lValue.AddRange(list);
            Assert.AreEqual(list.Count, lValue.Count);
            Assert.AreEqual(1, lValue.IndexOf(list[1]));

            bool fResult = lValue.Remove(new TestClass());
            bool tResult = lValue.Remove(list[0]);

            Assert.IsFalse(fResult);
            Assert.IsTrue(tResult);

            fResult = lValue.Contains(list[0]);

            Assert.IsFalse(fResult);
        }

        [TestMethod]
        public void DynamicListValue_MainCastTest()
        {
            //Initializing
            bool? boolNullDefault = true;
            bool boolDefault = true;
            string stringDefault = "This Is An String";
            int? intNullDefault = int.MaxValue - 10;
            int intDefault = int.MaxValue - 5;
            uint? uintNullDefault = uint.MaxValue - 10;
            uint uintDefault = uint.MaxValue - 5;
            short? shortIntNullDefault = short.MaxValue - 10;
            short shortIntDefault = short.MaxValue - 5;
            ushort? ushortIntNullDefault = ushort.MaxValue - 10;
            ushort ushortIntDefault = ushort.MaxValue - 5;
            Guid? guidNullDefault = Guid.NewGuid();
            Guid guidDefault = Guid.NewGuid();
            TimeSpan? timeSpanNullDefault = TimeSpan.FromSeconds(10);
            TimeSpan timeSpanDefault = TimeSpan.FromSeconds(5);
            DateTime? dateTimeNullDefault = DateTime.Now;
            DateTime dateTimeDefault = DateTime.UtcNow;
            long? longNullDefault = long.MaxValue - 10;
            long longDefault = long.MaxValue - 5;
            ulong? ulongNullDefault = ulong.MaxValue - 10;
            ulong ulongDefault = ulong.MaxValue - 5;
            float? floatNullDefault = 10.0F;
            float floatDefault = 5.3F;
            decimal? decimalNullDefault = 10.32m;
            decimal decimalDefault = 52.23m;
            double? doubleNullDefault = 2.5d;
            double doubleDefault = 36.12d;
            char? charNullDefault = 'd';
            char charDefault = 'e';

            TestClass testClassDefault = new TestClass();
            List<uint> listUintDefault = new List<uint> { 10, 23, 335, 456789 };
            List<TestClass> listTestClassDefault = new List<TestClass> { new TestClass(), new TestClass() };


            //Inbound casting
            DynamicListValue boolNullValue = boolNullDefault;
            DynamicListValue boolValue = boolDefault;
            DynamicListValue stringValue = stringDefault;
            DynamicListValue intNullValue = intNullDefault;
            DynamicListValue intValue = intDefault;
            DynamicListValue uintNullValue = uintNullDefault;
            DynamicListValue uintValue = uintDefault;
            DynamicListValue shortNullValue = shortIntNullDefault;
            DynamicListValue shortValue = shortIntDefault;
            DynamicListValue ushortNullValue = ushortIntNullDefault;
            DynamicListValue ushortValue = ushortIntDefault;
            DynamicListValue guidNullValue = guidNullDefault;
            DynamicListValue guidValue = guidDefault;
            DynamicListValue dateTimeNullValue = dateTimeNullDefault;
            DynamicListValue dateTimeValue = dateTimeDefault;
            DynamicListValue timespanNullValue = timeSpanNullDefault;
            DynamicListValue timespanValue = timeSpanDefault;
            DynamicListValue longNullValue = longNullDefault;
            DynamicListValue longValue = longDefault;
            DynamicListValue ulongNullValue = ulongNullDefault;
            DynamicListValue ulongValue = ulongDefault;
            DynamicListValue floatNullValue = floatNullDefault;
            DynamicListValue floatValue = floatDefault;
            DynamicListValue decimalNullValue = decimalNullDefault;
            DynamicListValue decimalValue = decimalDefault;
            DynamicListValue doubleNullValue = doubleNullDefault;
            DynamicListValue doubleValue = doubleDefault;
            DynamicListValue charNullValue = charNullDefault;
            DynamicListValue charValue = charDefault;

            DynamicListValue listUintValue = listUintDefault;
            DynamicListValue testClassValue = new DynamicListValue(testClassDefault);
            DynamicListValue listTestClassValue = new DynamicListValue(listTestClassDefault);

            //Tests
            Assert.IsNotNull(boolNullValue);
            Assert.IsTrue(boolNullValue.HasValue);
            Assert.AreEqual<bool?>(boolNullDefault, boolNullValue);

            Assert.IsNotNull(boolValue);
            Assert.IsTrue(boolValue.HasValue);
            Assert.AreEqual<bool>(boolDefault, boolValue);

            Assert.IsNotNull(stringValue);
            Assert.IsTrue(stringValue.HasValue);
            Assert.AreEqual<string>(stringDefault, stringValue);

            Assert.IsNotNull(intNullValue);
            Assert.IsTrue(intNullValue.HasValue);
            Assert.AreEqual<int?>(intNullDefault, intNullValue);

            Assert.IsNotNull(intValue);
            Assert.IsTrue(intValue.HasValue);
            Assert.AreEqual<int>(intDefault, intValue);

            Assert.IsNotNull(uintNullValue);
            Assert.IsTrue(uintNullValue.HasValue);
            Assert.AreEqual<uint?>(uintNullDefault, uintNullValue);

            Assert.IsNotNull(uintValue);
            Assert.IsTrue(uintValue.HasValue);
            Assert.AreEqual<uint>(uintDefault, uintValue);

            Assert.IsNotNull(shortNullValue);
            Assert.IsTrue(shortNullValue.HasValue);
            Assert.AreEqual<short?>(shortIntNullDefault, shortNullValue);

            Assert.IsNotNull(shortValue);
            Assert.IsTrue(shortValue.HasValue);
            Assert.AreEqual<short>(shortIntDefault, shortValue);

            Assert.IsNotNull(ushortNullValue);
            Assert.IsTrue(ushortNullValue.HasValue);
            Assert.AreEqual<ushort?>(ushortIntNullDefault, ushortNullValue);

            Assert.IsNotNull(ushortValue);
            Assert.IsTrue(ushortValue.HasValue);
            Assert.AreEqual<ushort>(ushortIntDefault, ushortValue);

            Assert.IsNotNull(guidNullValue);
            Assert.IsTrue(guidNullValue.HasValue);
            Assert.AreEqual<Guid?>(guidNullDefault, guidNullValue);

            Assert.IsNotNull(guidValue);
            Assert.IsTrue(guidValue.HasValue);
            Assert.AreEqual<Guid>(guidValue, guidValue);

            Assert.IsNotNull(dateTimeNullValue);
            Assert.IsTrue(dateTimeNullValue.HasValue);
            Assert.AreEqual<DateTime?>(dateTimeNullDefault, dateTimeNullValue);

            Assert.IsNotNull(dateTimeValue);
            Assert.IsTrue(dateTimeValue.HasValue);
            Assert.AreEqual<DateTime>(dateTimeDefault, dateTimeValue);

            Assert.IsNotNull(timespanNullValue);
            Assert.IsTrue(timespanNullValue.HasValue);
            Assert.AreEqual<TimeSpan?>(timeSpanNullDefault, timespanNullValue);

            Assert.IsNotNull(timespanValue);
            Assert.IsTrue(timespanValue.HasValue);
            Assert.AreEqual<TimeSpan>(timeSpanDefault, timespanValue);

            Assert.IsNotNull(longNullValue);
            Assert.IsTrue(longNullValue.HasValue);
            Assert.AreEqual<long?>(longNullDefault, longNullValue);

            Assert.IsNotNull(longValue);
            Assert.IsTrue(longValue.HasValue);
            Assert.AreEqual<long>(longDefault, longValue);

            Assert.IsNotNull(ulongNullValue);
            Assert.IsTrue(ulongNullValue.HasValue);
            Assert.AreEqual<ulong?>(ulongNullDefault, ulongNullValue);

            Assert.IsNotNull(ulongValue);
            Assert.IsTrue(ulongValue.HasValue);
            Assert.AreEqual<ulong>(ulongDefault, ulongValue);

            Assert.IsNotNull(floatNullValue);
            Assert.IsTrue(floatNullValue.HasValue);
            Assert.AreEqual<float?>(floatNullDefault, floatNullValue);

            Assert.IsNotNull(floatValue);
            Assert.IsTrue(floatValue.HasValue);
            Assert.AreEqual<float>(floatDefault, floatValue);

            Assert.IsNotNull(decimalNullValue);
            Assert.IsTrue(decimalNullValue.HasValue);
            Assert.AreEqual<decimal?>(decimalNullDefault, decimalNullValue);

            Assert.IsNotNull(decimalValue);
            Assert.IsTrue(decimalValue.HasValue);
            Assert.AreEqual<decimal>(decimalDefault, decimalValue);

            Assert.IsNotNull(doubleNullValue);
            Assert.IsTrue(doubleNullValue.HasValue);
            Assert.AreEqual<double?>(doubleNullDefault, doubleNullValue);

            Assert.IsNotNull(doubleValue);
            Assert.IsTrue(doubleValue.HasValue);
            Assert.AreEqual<double>(doubleDefault, doubleValue);

            Assert.IsNotNull(charNullValue);
            Assert.IsTrue(charNullValue.HasValue);
            Assert.AreEqual<char?>(charNullDefault, charNullValue);

            Assert.IsNotNull(charValue);
            Assert.IsTrue(charValue.HasValue);
            Assert.AreEqual<char>(charDefault, charValue);

            Assert.IsNotNull(listUintValue);
            Assert.AreEqual(listUintDefault.Count, listUintValue.Count);
            List<uint> castListUint = listUintValue;
            Assert.AreEqual(listUintDefault.Count, castListUint.Count);

            Assert.IsNotNull(testClassValue);
            Assert.IsTrue(testClassValue.HasValue);
            Assert.AreEqual(testClassDefault, testClassValue.Cast<TestClass>());

            Assert.IsNotNull(listTestClassValue);
            Assert.AreEqual(listTestClassDefault.Count, listTestClassValue.Count);
            List<TestClass> castListTestClass = listTestClassValue.CastList<TestClass>();
            Assert.AreEqual(listTestClassDefault.Count, castListTestClass.Count);
        }

        [TestMethod]
        public void DynamicListValue_RaiseEventTest()
        {
            int countAddEvent = 0;
            int countChangeEvent = 0;
            int countOrderEvent = 0;
            int countRemoveEvent = 0;
            int countClearEvent = 0;
            int otherEventType = 0;

            string mainValue = "Main Test Subject";
            List<string> values = new List<string> { "val1", "val2", "val3" };

            DynamicListValue obj = new DynamicListValue();

            obj.OnDynamicListValueChanged += (sender, e) =>
            {
                switch (e.EventType)
                {
                    case DynamicDictionaryChangedType.AddedValue:
                        countAddEvent++;
                        break;

                    case DynamicDictionaryChangedType.ChangedValue:
                        countChangeEvent++;
                        break;

                    case DynamicDictionaryChangedType.OrderValue:
                        countOrderEvent++;
                        break;

                    case DynamicDictionaryChangedType.RemovedValue:
                        countRemoveEvent++;
                        break;

                    case DynamicDictionaryChangedType.Clear:
                        countClearEvent++;
                        break;

                    default:
                        otherEventType++;
                        break;
                }
            };

            obj.Value = mainValue;
            Assert.AreEqual(1, countAddEvent);

            obj.AddRange(values);
            Assert.AreEqual(4, countAddEvent);

            obj.Add(mainValue);
            Assert.AreEqual(5, countAddEvent);

            obj.SetMainValue(1);
            Assert.AreEqual(1, countOrderEvent);

            obj.SetMainValue(obj[2]);
            Assert.AreEqual(2, countOrderEvent);

            obj[0] = mainValue;
            obj[1] = mainValue;
            obj[2] = mainValue;
            Assert.AreEqual(2, countChangeEvent);

            obj.RemoveAt(0);
            Assert.AreEqual(1, countRemoveEvent);

            obj.RemoveRange(values);
            Assert.AreEqual(2, countRemoveEvent);

            obj.Clear();
            Assert.AreEqual(1, countClearEvent);

            Assert.AreEqual(0, otherEventType);
        }
    }
}
