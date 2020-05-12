using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monitoring.Validators;

namespace Monitoring.Test
{
    [TestClass]
    public class StringValidatorTests
    {
        [TestMethod]
        public void ValidateMaxLength_ValidateNullString_ErrorEmptyStringReturned()
        {
            //Arrange
            StringValidator stringValidator = new StringValidator();
            string testStringForValidate = null;
            string testName = "Name";
            List<string> expected = new List<string> { "Поле Name должно быть заполнено!" };

            //Act
            List<string> result = stringValidator.ValidateMaxLength(testStringForValidate, testName);
     
            //Assert
            Assert.AreEqual(expected[0], result[0]);
        }

        [TestMethod]
        public void ValidateMaxLength_ValidateEmptyString_ErrorEmptyStringReturned()
        {
            //Arrange
            StringValidator stringValidator = new StringValidator();
            string testStringForValidate = "";
            string testName = "Name";
            List<string> expected = new List<string> { "Поле Name должно быть заполнено!" };

            //Act
            List<string> result = stringValidator.ValidateMaxLength(testStringForValidate, testName);

            //Assert
            Assert.AreEqual(expected[0], result[0]);
        }

        [TestMethod]
        public void ValidateMaxLength_ValidateBeyondMaxLengthString_ErrorMaxLengthStringReturned()
        {
            //Arrange
            StringValidator stringValidator = new StringValidator();
            string testStringForValidate = "12345678901234567890123456789012345";
            string testName = "Name";
            List<string> expected = new List<string> { $"Длина строки {testName} должна быть  до 32 символов!" };

            //Act
            List<string> result = stringValidator.ValidateMaxLength(testStringForValidate, testName);

            //Assert
            Assert.AreEqual(expected[0], result[0]);
        }

        [TestMethod]
        public void ValidateMaxLength_ValidateNotEmptyAndNotBeyondMaxString_EmptyErrorListReturned()
        {
            //Arrange
            StringValidator stringValidator = new StringValidator();
            string testStringForValidate = "testNormalString";
            string testName = "Name";
            List<string> expected = new List<string>();

            //Act
            List<string> result = stringValidator.ValidateMaxLength(testStringForValidate, testName);

            //Assert
            Assert.AreEqual(expected.Count, result.Count);
        }

    }
}
