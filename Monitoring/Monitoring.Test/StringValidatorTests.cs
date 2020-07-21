using System.Collections.Generic;
using ApplicationCore.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Monitoring.Test
{
    [TestClass]
    public class StringValidatorTests
    {
        //[TestMethod]
        //public void Validators_ValidateMaxLength_ValidateNullString_ErrorEmptyStringReturned()
        //{
        //    //Arrange
        //    Validators validators = new Validators();
        //    List<ValidationData> data = new List<ValidationData>() 
        //    {
        //        new ValidationData { Name = "Name", Value = null }
        //    };
        //    string expected = "Поле Name должно быть заполнено!";

        //    //Act
        //    string result = validators.ValidateAll(data);
     
        //    //Assert
        //    Assert.AreEqual(expected[0], result[0]);
        //}

        //[TestMethod]
        //public void StringValidator_ValidateMaxLength_ValidateEmptyString_ErrorEmptyStringReturned()
        //{
        //    //Arrange
        //    Validators validators = new Validators();
        //    string testStringForValidate = "";
        //    string testName = "Name";
        //    List<string> expected = new List<string> { "Поле Name должно быть заполнено!" };

        //    //Act
        //    List<string> result = stringValidator.ValidateMaxLength(testStringForValidate, testName);

        //    //Assert
        //    Assert.AreEqual(expected[0], result[0]);
        //}

        //[TestMethod]
        //public void StringValidator_ValidateMaxLength_ValidateBeyondMaxLengthString_ErrorMaxLengthStringReturned()
        //{
        //    //Arrange
        //    StringValidator stringValidator = new StringValidator();
        //    string testStringForValidate = "12345678901234567890123456789012345";
        //    string testName = "Name";
        //    List<string> expected = new List<string> { $"Длина строки {testName} должна быть  до 32 символов!" };

        //    //Act
        //    List<string> result = stringValidator.ValidateMaxLength(testStringForValidate, testName);

        //    //Assert
        //    Assert.AreEqual(expected[0], result[0]);
        //}

        //[TestMethod]
        //public void StringValidator_ValidateMaxLength_ValidateNotEmptyAndNotBeyondMaxString_EmptyErrorListReturned()
        //{
        //    //Arrange
        //    StringValidator stringValidator = new StringValidator();
        //    string testStringForValidate = "testNormalString";
        //    string testName = "Name";
        //    List<string> expected = new List<string>();

        //    //Act
        //    List<string> result = stringValidator.ValidateMaxLength(testStringForValidate, testName);

        //    //Assert
        //    Assert.AreEqual(expected.Count, result.Count);
        //}

    }
}
