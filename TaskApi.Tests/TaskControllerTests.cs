using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApi.Controllers;
using TaskApi.Data;
using TaskApi.Data.Repositories;
using TaskApi.DTOs;
using TaskApi.Entities;
using TaskApi.Helpers;
using TaskApi.Interfaces;

namespace TaskApi.Tests
{
    public class TaskControllerTests
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private Mock<DataContext> _context;
        private TaskController _taskController;
        private int _dataCount;

        [SetUp]
        public void Setup()
        {
            var list = new List<TaskEntity>
            {
                new TaskEntity { Id = 1, Title = "Yoga", Description = "stretching" },
                new TaskEntity { Id = 2, Title = "Buy food..", Description = "..for dog", Deadline = DateTime.Now.AddDays(2) }
            }.AsQueryable();

            _dataCount = list.Count();

            var data = list.BuildMockDbSet();

            _context = new Mock<DataContext>();
            _context.Setup(x => x.SaveChangesAsync(new System.Threading.CancellationToken())).Returns(Task.FromResult(1));
            _context.Setup(x => x.Tasks)
                .Returns(data.Object);

            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }));

            _unitOfWork = new UnitOfWork(_context.Object, _mapper);
            _taskController = new TaskController(_unitOfWork);
        }

        [Test]
        public async Task Should_Return_Valid_Type_When_Get_Tasks_Called()
        {
            var data = await _taskController.GetTasks();

            var result = data.Result;

            Assert.NotNull(result);
            Assert.IsAssignableFrom<OkObjectResult>(result);

            var resultValue = ((OkObjectResult)result).Value;
            Assert.NotNull(resultValue);
            Assert.IsAssignableFrom<List<TaskEntity>>(resultValue);
        }

        [Test]
        public async Task Should_Return_Correct_Count_Of_Data_When_Get_Tasks_Called()
        {
            var data = await _taskController.GetTasks();

            var result = ((OkObjectResult)data.Result).Value as List<TaskEntity>;

            Assert.AreEqual(_dataCount, result.Count);
        }

        [Test]
        public async Task Should_Return_Not_Found_When_Parameter_Get_Task_Doesnt_Match()
        {
            var data = await _taskController.GetTask(-1);

            var result = data.Result;

            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task Should_Return_Valid_Type_When_Get_Task_Called()
        {
            var data = await _taskController.GetTask(1);

            var result = data.Result;

            Assert.NotNull(result);
            Assert.IsAssignableFrom<OkObjectResult>(result);

            var resultValue = ((OkObjectResult)result).Value;
            Assert.NotNull(resultValue);
            Assert.IsAssignableFrom<TaskEntity>(resultValue);
        }

        [Test]
        public async Task Should_Return_Valid_Type_When_Add_Task_Called()
        {
            var postData = new TaskDto
            {
                Title = "Test title",
                Description = "Test descripton",
                Deadline = DateTime.Now.AddMonths(1)
            };
            var data = await _taskController.AddTask(postData);

            Assert.NotNull(data);
            Assert.IsAssignableFrom<CreatedAtActionResult>(data);
        }

        [Test]
        public async Task Should_Return_Conflict_When_Update_Task_Called()
        {
            var putData = new TaskEntity
            {
                Id = -1,
                Title = "Test title",
                Description = "Test descripton",
                Deadline = DateTime.Now.AddMonths(1)
            };
            var data = await _taskController.UpdateTask(1, putData);

            Assert.IsAssignableFrom<ConflictObjectResult>(data);
        }

        [Test]
        public async Task Should_Return_Not_Found_When_Update_Task_Called()
        {
            var putData = new TaskEntity
            {
                Id = -1,
                Title = "Test title",
                Description = "Test descripton",
                Deadline = DateTime.Now.AddMonths(1)
            };
            var data = await _taskController.UpdateTask(-1, putData);

            Assert.IsAssignableFrom<NotFoundObjectResult>(data);
        }

        [Test]
        public async Task Should_Return_Not_Found_When_Delete_Task_Called()
        {
            var data = await _taskController.DeleteTask(-1);

            Assert.IsAssignableFrom<NotFoundObjectResult>(data);
        }

        [Test]
        public async Task Should_Return_Ok_When_Delete_Task_Called()
        {
            var data = await _taskController.DeleteTask(1);

            Assert.IsAssignableFrom<OkResult>(data);
        }
    }
}