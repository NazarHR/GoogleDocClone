using GoogleDocClone.Data;
using GoogleDocClone.Entities;
using GoogleDocClone.Services;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System.Linq;


namespace GoogleDocClone.Tests
{
    public class DocumentsRepositoryTests
    {
        const string validUserId = "ValidId";
        const string otherValidUserId = "otherValidUserId";
        const string invalidUserId = "InvalidUserId";
        
        const string title = "TIile";
        List<Document> testDataList;
        public DocumentsRepositoryTests() {

            testDataList = new List<Document>{
                new Document{Id = 1,UserId = validUserId,Title = title,Content = String.Empty,},
                new Document{Id = 2,UserId = validUserId,Title = title,Content = String.Empty,},
                new Document{Id = 3,UserId = otherValidUserId,Title = title,Content = String.Empty,}
            };
        }

        [Fact]
        public async Task GetUsersDocuments_InvalidUserId_ReturnsEmptyListAsync()
        {
            //Arrange
            var contextMock = new Mock<DocumentsDbContext>();
            contextMock.Setup(x => x.Documents).ReturnsDbSet(testDataList);
            var documentsRepository = new DocumentsRepository(contextMock.Object);
            //Act

            var result = await documentsRepository.GetUsersDocuments(invalidUserId);

            //Assert

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsersDocuments_ValidUserId_ReturnsRightNumberOFDocumentsAsync()
        {
            //Arrange
            var contextMock = new Mock<DocumentsDbContext>();
            contextMock.Setup(x => x.Documents).ReturnsDbSet(testDataList);
            var documentsRepository = new DocumentsRepository(contextMock.Object);
            //Act

            var result = await documentsRepository.GetUsersDocuments(validUserId);

            //Assert

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddDocument_RitghtTypeObject_IsAddedToDatabase_Async()
        {
            //Arrange
            var contextMock = new Mock<DocumentsDbContext>();
            contextMock.Setup(x => x.Add(It.IsAny<Document>())).
                Callback<Document>(d => testDataList.Add(d)).
                Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Document>)null);
            contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            var documentsRepository = new DocumentsRepository(contextMock.Object);
            var documentToAdd = new Document { Id = 4, UserId = validUserId, Title = title, Content = String.Empty };
            //Act
            await documentsRepository.AddDocument(documentToAdd);
            //Assert

            Assert.Equal(4, testDataList.Count());
            var addedDocument = testDataList.FirstOrDefault(d=>d.Id== 4);
            Assert.Equal(documentToAdd, addedDocument);
        }

        [Fact]
        public void DocumentExists_ExistingAndNotExistingDocuments_ReturnsTrueAndFalse()
        {
            //Arrange
            var contextMock = new Mock<DocumentsDbContext>();
            contextMock.Setup(x => x.Documents).ReturnsDbSet(testDataList);
            var documentsRepository = new DocumentsRepository(contextMock.Object);
            int existingDocumentID = 1;
            int notExistingDocumentId = 9999;

            //Act
            var existingDocumentResul = documentsRepository.DocumentExists(existingDocumentID);
            var notExistingDocumentResul = documentsRepository.DocumentExists(notExistingDocumentId);

            //Assert
            Assert.True(existingDocumentResul);
            Assert.False(notExistingDocumentResul);
        }
        
        [Fact]
        public async Task DeleteDocumentAsync_NullDocument_OmmitsRemoveCall_Async()
        {
            //Arrange
            var contextMock = new Mock<DocumentsDbContext>();
            contextMock.Setup(x => x.Documents.Remove(It.IsAny<Document>())).Throws<Exception>();
            var documentsRepository = new DocumentsRepository(contextMock.Object);

            //Act
            var exception = await Record.ExceptionAsync(()=>documentsRepository.DeleteDocumentAsync(null));

            //Assert

            Assert.Null(exception);
        }
    }

}