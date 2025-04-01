using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.CollectionRequest;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;
using PoemTown.Service.Interfaces;
using System.Security.Claims;
using PoemTown.Service.QueryOptions.FilterOptions.StatisticFilters;

namespace PoemTown.API.Controllers
{
    public class StatisticsController : BaseController
    {
        private readonly IStatisticService _service;
        private readonly IMapper _mapper;
        public StatisticsController(IMapper mapper, IStatisticService service)
        {
            _mapper = mapper;
            _service = service;
        }

        /// <summary>
        /// Lấy thống kê của người dùng, không yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// 
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("v1")]
        public async Task<ActionResult<BaseResponse>> GetStatistics()
        {
            var userClaim = User.Claims.FirstOrDefault(p => p.Type == "UserId");
            Guid? userId = null;
            if (userClaim != null)
            {
                userId = Guid.Parse(userClaim.Value);
            }
            var result = await _service.GetStatisticsAsync(userId);
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get statistic user successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê trên trang dashbaord, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/total")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetTotalStatisticResponse>>> GetTotalStatistic()
        {
            var result = await _service.GetTotalStatistic();
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get total statistic successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê người dùng online, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <remarks>
        /// period:
        ///
        /// - ByDate = 1 (lấy theo 30 ngày gần nhất),
        /// - ByMonth = 2 (lấy theo tháng chỉ trong năm hiện tại),
        /// - ByYear = 3 (lấy theo 5 năm gần nhất)
        /// - By15Days = 4 (lấy theo 15 ngày gần nhất)
        /// </remarks>
        /// <remarks></remarks>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/online-users")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetOnlineUserStatisticResponse>>>
            GetOnlineUserStatistic(GetOnlineUserFilterOption filter)
        {
            var result = await _service.GetOnlineUserStatistic(filter);
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get online user statistic successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê số lượng bài thơ được tải lên, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <remarks>
        /// period:
        ///
        /// - ByDate = 1 (lấy theo 30 ngày gần nhất),
        /// - ByMonth = 2 (lấy theo tháng chỉ trong năm hiện tại),
        /// - ByYear = 3 (lấy theo 5 năm gần nhất)
        /// - By15Days = 4 (lấy theo 15 ngày gần nhất)
        /// </remarks>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/poem-uploads")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetPoemUploadStatisticResponse>>>
            GetUploadPoemStatistic(GetPoemUploadFilterOption filter)
        {
            var result = await _service.GetUploadPoemStatistic(filter);
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get upload poem statistic successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê số lượng bài thơ theo từng loại, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <remarks>
        /// Type:
        ///
        /// - ThoTuDo = 1,
        /// - ThoLucBat = 2,
        /// - ThoSongThatLucBat = 3,
        /// - ThoThatNgonTuTuyet = 4,
        /// - ThoNguNgonTuTuyet = 5,
        /// - ThoThatNgonBatCu = 6,
        /// - ThoBonChu = 7,
        /// - ThoNamChu = 8,
        /// - ThoSauChu = 9,
        /// - ThoBayChu = 10,
        /// - ThoTamChu = 11,
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/poem-types")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetPoemTypeStatisticResponse>>> GetPoemTypeStatistic()
        {
            var result = await _service.GetPoemTypeStatistic();
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get poem type statistic successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê số lượng bài thơ bị báo cáo, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <remarks>
        /// ReportStatus:
        /// 
        /// - Pending = 1,
        /// - Approved = 2,
        /// - Rejected = 3
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/report-poems")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetReportPoemStatisticResponse>>> GetReportPoemStatistic()
        {
            var result = await _service.GetReportPoemStatistic();
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get report poem statistic successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê số lượng người dùng bị báo cáo, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <remarks>
        /// ReportStatus:
        /// 
        /// - Pending = 1,
        /// - Approved = 2,
        /// - Rejected = 3
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/report-users")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetReportUserStatisticResponse>>> GetReportUserStatistic()
        {
            var result = await _service.GetReportUserStatistic();
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get report user statistic successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê số lượng bài thơ bị báo cáo là ĐẠO VĂN, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <remarks>
        /// ReportStatus:
        /// 
        /// - Pending = 1,
        /// - Approved = 2,
        /// - Rejected = 3
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/report-plagiarism-poems")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetReportPoemStatisticResponse>>>
            GetReportPlagiarismPoemStatistic()
        {
            var result = await _service.GetReportPlagiarismPoemStatistic();
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get report plagiarism poem statistic successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê số lượng giao dịch, tổng số tiền đã giao dịch, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <remarks>
        /// period:
        ///
        /// - ByDate = 1 (lấy theo 30 ngày gần nhất),
        /// - ByMonth = 2 (lấy theo tháng chỉ trong năm hiện tại),
        /// - ByYear = 3 (lấy theo 5 năm gần nhất)
        /// - By15Days = 4 (lấy theo 15 ngày gần nhất)
        /// </remarks>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/transactions")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetTransactionStatisticResponse>>>
            GetTransactionStatistic([FromQuery] GetTransactionStatisticFilterOption filter)
        {
            var result = await _service.GetTransactionStatistic(filter);
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get transaction statistic successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê số lượng đơn hàng theo từng trạng thái, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <remarks>
        /// OrderStatus:
        ///
        /// - Pending = 1,
        /// - Paid = 2,
        /// - Cancelled = 3
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/order-status")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetOrderStatusStatisticResponse>>>
            GetOrderStatusStatistic()
        {
            var result = await _service.GetOrderStatusStatistic();
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get order type statistic successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê số lượng đơn hàng đã được thánh toán theo từng loại Master Templates, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/master-template-orders")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetMasterTemplateOrderStatisticResponse>>>
            GetMasterTemplateOrderStatistic()
        {
            var result = await _service.GetMasterTemplateOrderStatistic();
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get master template order statistic successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê số lượng đơn hàng đã được thánh toán theo từng loại, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <remarks>
        /// orderTypes:
        ///
        /// - EWalletDeposit = 1,
        /// - MasterTemplates = 2,
        /// - RecordFiles = 3,
        /// - Poems = 4,
        /// </remarks>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/order-types")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetOrderTypeStatisticResponse>>> GetOrderTypeStatistic()
        {
            var result = await _service.GetOrderTypeStatistic();
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get order type statistic successfully", result));
        }
        
        /// <summary>
        /// Lấy thống kê doanh thu, yêu cầu đăng nhập dưới quyền ADMIN
        /// </summary>
        /// <remarks>
        /// incomeType:
        ///
        /// - EWalletDeposit = 1,
        /// - MasterTemplates = 2,
        /// 
        /// period:
        ///
        /// - ByDate = 1 (lấy theo 30 ngày gần nhất),
        /// - ByMonth = 2 (lấy theo tháng chỉ trong năm hiện tại),
        /// - ByYear = 3 (lấy theo 5 năm gần nhất)
        /// - By15Days = 4 (lấy theo 15 ngày gần nhất)
        /// </remarks>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/incomes")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseResponse<GetIncomeStatisticResponse>>>
            GetIncomeStatistic([FromQuery] GetIncomeStatisticFilterOption filter)
        {
            var result = await _service.GetIncomeStatistic(filter);
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get income statistic successfully", result));
        }
    }
}
