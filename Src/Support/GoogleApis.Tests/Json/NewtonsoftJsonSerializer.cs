﻿/*
Copyright 2017 Google Inc

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using Google.Apis.Json;
using Newtonsoft.Json;
using NUnit.Framework;
using System;

namespace Google.Apis.Tests.Json
{
    class NewtonsoftJsonSerializerTest
    {
        [Test]
        public void CreateDefaultSettings_Independent()
        {
            var settings1 = NewtonsoftJsonSerializer.CreateDefaultSettings();
            var settings2 = NewtonsoftJsonSerializer.CreateDefaultSettings();

            settings1.DateParseHandling = DateParseHandling.DateTimeOffset;
            settings2.DateParseHandling = DateParseHandling.None;
            // Not affected by changing settings2
            Assert.AreEqual(DateParseHandling.DateTimeOffset, settings1.DateParseHandling);
        }

        [Test]
        public void InstanceIsCached()
        {
            var instance1 = NewtonsoftJsonSerializer.Instance;
            var instance2 = NewtonsoftJsonSerializer.Instance;
            Assert.AreSame(instance1, instance2);
        }

        [Test]
        public void DefaultInstanceParsesDates()
        {
            string text = "\"2017-05-03T16:38:00Z\"";
            object value = NewtonsoftJsonSerializer.Instance.Deserialize<object>(text);
            Assert.IsInstanceOf<DateTime>(value);
        }

        [Test]
        public void CustomInstanceAvoidingDateParsin()
        {
            string text = "\"2017-05-03T16:38:00Z\"";
            var settings = NewtonsoftJsonSerializer.CreateDefaultSettings();
            settings.DateParseHandling = DateParseHandling.None;
            var instance = new NewtonsoftJsonSerializer(settings);
            object value = instance.Deserialize<object>(text);

            // No magic parsing to DateTime...
            Assert.IsInstanceOf<string>(value);
        }
    }
}
