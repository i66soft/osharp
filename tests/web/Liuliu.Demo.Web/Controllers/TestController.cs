﻿// -----------------------------------------------------------------------
//  <copyright file="TestController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Liuliu.Demo.Identity;
using Liuliu.Demo.Identity.Dtos;
using Liuliu.Demo.Identity.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using OSharp.AspNetCore;
using OSharp.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Collections;
using OSharp.Data;
using OSharp.Dependency;
using OSharp.Net;

namespace Liuliu.Demo.Web.Controllers
{
    [Description("网站-测试")]
    [ClassFilter]
    public class TestController : ApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly IIdentityContract _identityContract;
        private readonly IEmailSender _emailSender;

        public TestController(UserManager<User> userManager, IIdentityContract identityContract, IEmailSender emailSender)
        {
            _userManager = userManager;
            _identityContract = identityContract;
            _emailSender = emailSender;
        }

        [HttpGet]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [MethodFilter]
        [Description("测试01")]
        public async Task<string> Test01()
        {
            List<object> list = new List<object>();

            if (!_userManager.Users.Any())
            {
                RegisterDto dto = new RegisterDto
                {
                    UserName = "admin",
                    Password = "gmf31529019",
                    ConfirmPassword = "gmf31529019",
                    Email = "i66soft@qq.com",
                    NickName = "大站长",
                    RegisterIp = HttpContext.GetClientIp()
                };

                OperationResult<User> result = await _identityContract.Register(dto);
                if (result.Successed)
                {
                    User user = result.Data;
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }
                list.Add(result.Message);

                dto = new RegisterDto()
                {
                    UserName ="mf.guo",
                    Password = "gmf31529019",
                    Email = "mf.guo@qq.com",
                    NickName = "柳柳英侠",
                    RegisterIp = HttpContext.GetClientIp()
                };
                result = await _identityContract.Register(dto);
                if (result.Successed)
                {
                    User user = result.Data;
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }
                list.Add(result.Message);
            }

            return list.ExpandAndToString("\r\n");
        }

        [HttpGet]
        public async Task<bool> SendEmail()
        {
            string body =
                $"亲爱的用户 <strong>ArcherTrister</strong>[LX]，您好！<br>"
                + $"欢迎注册，激活邮箱请 <a href=\"www.lxking.cn\" target=\"_blank\"><strong>点击这里</strong></a><br>"
                + $"如果上面的链接无法点击，您可以复制以下地址，并粘贴到浏览器的地址栏中打开。<br>"
                + $"www.lxking.cn<br>"
                + $"祝您使用愉快！";
            await _emailSender.SendEmailAsync("319807406@qq.com", "乐讯网络科技", body);
            return true;
        }
    }


    public class ClassFilter : ActionFilterAttribute, IExceptionFilter
    {
        private readonly ILogger _logger;

        public ClassFilter()
        {
            _logger = ServiceLocator.Instance.GetLogger(GetType());
        }

        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("ClassFilter - OnActionExecuting");
        }

        /// <inheritdoc />
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("ClassFilter - OnActionExecuted");
        }

        /// <inheritdoc />
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            _logger.LogInformation("ClassFilter - OnResultExecuting");
        }

        /// <inheritdoc />
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            _logger.LogInformation("ClassFilter - OnResultExecuted");
        }

        /// <summary>
        /// Called after an action has thrown an <see cref="T:System.Exception" />.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ExceptionContext" />.</param>
        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception.Message;
            _logger.LogInformation("ClassFilter - OnException");
        }
    }

    public class MethodFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public MethodFilter()
        {
            _logger = ServiceLocator.Instance.GetLogger(GetType());
        }

        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("MethodFilter - OnActionExecuting");
        }

        /// <inheritdoc />
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("MethodFilter - OnActionExecuted");
        }

        /// <inheritdoc />
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            _logger.LogInformation("MethodFilter - OnResultExecuting");
        }

        /// <inheritdoc />
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            _logger.LogInformation("MethodFilter - OnResultExecuted");
        }

    }
}