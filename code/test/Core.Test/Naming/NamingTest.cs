﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;

using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    public class NamingTest
    {
        private TemplatesFixture _fixture;

        public NamingTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void Infer_Existing(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { "App" };
            var validators = new List<Validator>()
            {
                new ExistingNamesValidator(existing)
            };
            var result = Naming.Infer("App", validators);

            Assert.Equal("App1", result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void Infer_Reserved(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { };
            var validators = new List<Validator>()
            {
                new ReservedNamesValidator()
            };
            var result = Naming.Infer("Page", validators);

            Assert.Equal("Page1", result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void Infer_Default(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { };
            var validators = new List<Validator>()
            {
                new DefaultNamesValidator()
            };
            var result = Naming.Infer("LiveTile", validators);

            Assert.Equal("LiveTile1", result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void Infer_Clean(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { };
            var result = Naming.Infer("Blank$Page", new List<Validator>());

            Assert.Equal("BlankPage", result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void Infer_Clean2(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { };
            var result = Naming.Infer("ÑäöÜ!Page", new List<Validator>());

            Assert.Equal("ÑäöÜPage", result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void Infer_TitleCase(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { };
            var result = Naming.Infer("blank page", new List<Validator>());

            Assert.Equal("BlankPage", result);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void Validate(string language)
        {
            SetUpFixtureForTesting(language);

            var result = Naming.Validate("Blank1", new List<Validator>());

            Assert.True(result.IsValid);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void Validate_Empty(string language)
        {
            SetUpFixtureForTesting(language);

            var result = Naming.Validate("", new List<Validator>());

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.Empty, result.ErrorType);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void Validate_Existing(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { "Blank" };
            var validators = new List<Validator>()
            {
                new ExistingNamesValidator(existing)
            };
            var result = Naming.Validate("Blank", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.AlreadyExists, result.ErrorType);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void Validate_Default(string language)
        {
            SetUpFixtureForTesting(language);

            var validators = new List<Validator>
            {
                new DefaultNamesValidator()
            };
            var result = Naming.Validate("LiveTile", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.ReservedName, result.ErrorType);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void Validate_Reserved(string language)
        {
            SetUpFixtureForTesting(language);

            var validators = new List<Validator>()
            {
                new ReservedNamesValidator()
            };
            var result = Naming.Validate("Page", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.ReservedName, result.ErrorType);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void Validate_BadFormat_InvalidChars(string language)
        {
            SetUpFixtureForTesting(language);

            var existing = new string[] { };
            var result = Naming.Validate("Blank;", new List<Validator>());

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.BadFormat, result.ErrorType);
        }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void Validate_BadFormat_StartWithNumber(string language)
        {
            SetUpFixtureForTesting(language);

            var result = Naming.Validate("1Blank", new List<Validator>());

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.BadFormat, result.ErrorType);
        }

        private ITemplateInfo GetTarget(string templateName)
        {
            var allTemplates = GenContext.ToolBox.Repo.GetAll();
            var target = allTemplates.FirstOrDefault(t => t.Name == templateName);
            if (target == null)
            {
                throw new ArgumentException($"There is no template with name '{templateName}'. Number of templates: '{allTemplates.Count()}'");
            }
            return target;
        }

        private void SetUpFixtureForTesting(string language)
        {
            _fixture.InitializeFixture(language);
        }

        public static IEnumerable<object[]> GetAllLanguages()
        {
            foreach (var language in Language.GetAllLanguages())
            {
                yield return new object[] { language };
            }
        }
    }
}
