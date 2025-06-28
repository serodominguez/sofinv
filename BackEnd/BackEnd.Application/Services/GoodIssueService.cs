using BackEnd.Application.Dtos.DetailsIssueDtos;
using BackEnd.Application.Dtos.GoodsIssueDtos;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;

namespace BackEnd.Application.Services
{
    public class GoodIssueService
    {
        private readonly IGoodsIssueRepository _goodsIssueRepository;

        public GoodIssueService(IGoodsIssueRepository goodsIssueRepository)
        {
            _goodsIssueRepository = goodsIssueRepository;
        }

        public async Task<string> CreateIssue(GoodIssueCreateDto createDto)
        {
            var issue = new GoodsIssue
            {
                DATE_SALE = createDto.DATE_SALE,
                TYPE_ISSUE = createDto.TYPE_ISSUE,
                TYPE_DOCUMENT = createDto.TYPE_DOCUMENT,
                DOCUMENT_NUMBER = createDto.DOCUMENT_NUMBER,
                ANNOTATIONS = createDto.ANNOTATIONS,
                PK_CUSTOMER = createDto.PK_CUSTOMER,
                PK_USER = createDto.PK_USER,
                PK_WAREHOUSE = createDto.PK_WAREHOUSE,
                details = createDto.details?.Select(d => new DetailsIssue
                {
                    PK_PRODUCT = d.PK_PRODUCT,
                    QUANTITY = d.QUANTITY,
                    PRICE = d.PRICE
                }).ToList()
            };

            return await _goodsIssueRepository.CreateIssue(issue);
        }

        public async Task<IEnumerable<GoodIssueReadDto>> ReadIssues(int id)
        {
            var issues = await _goodsIssueRepository.ReadIssues(id);
            return issues.Select(issue => new GoodIssueReadDto
            {
                PK_ISSUE = issue.PK_ISSUE,
                CODE = issue.CODE,
                DATE_SALE = issue.DATE_SALE.HasValue ? issue.DATE_SALE.Value.ToString("dd/MM/yyyy") : null,
                DATE_REGISTRATION = issue.DATE_REGISTRATION.HasValue ? issue.DATE_REGISTRATION.Value.ToString("dd/MM/yyyy HH:mm") : null,
                TYPE_ISSUE = issue.TYPE_ISSUE,
                TYPE_DOCUMENT = issue.TYPE_DOCUMENT,
                DOCUMENT_NUMBER = issue.DOCUMENT_NUMBER,
                ANNOTATIONS = issue.ANNOTATIONS,
                STATUS = issue.STATUS,
                PK_CUSTOMER = issue.PK_CUSTOMER,
                PK_USER = issue.PK_USER,
                PK_WAREHOUSE = issue.PK_WAREHOUSE,
                NAMES = issue.NAMES,
                USER_NAME = issue.USER_NAME,
                STORE_NAME = issue.STORE_NAME
            });
        }

        public async Task<IEnumerable<GoodIssueReadDto>> SearchIssue(string text, int id)
        {
            var issues = await _goodsIssueRepository.SearchIssue(text, id);
            return issues.Select(issue => new GoodIssueReadDto
            {
                PK_ISSUE = issue.PK_ISSUE,
                CODE = issue.CODE,
                DATE_SALE = issue.DATE_SALE.HasValue ? issue.DATE_SALE.Value.ToString("dd/MM/yyyy") : null,
                DATE_REGISTRATION = issue.DATE_REGISTRATION.HasValue ? issue.DATE_REGISTRATION.Value.ToString("dd/MM/yyyy HH:mm") : null,
                TYPE_ISSUE = issue.TYPE_ISSUE,
                TYPE_DOCUMENT = issue.TYPE_DOCUMENT,
                DOCUMENT_NUMBER = issue.DOCUMENT_NUMBER,
                ANNOTATIONS = issue.ANNOTATIONS,
                STATUS = issue.STATUS,
                PK_CUSTOMER = issue.PK_CUSTOMER,
                PK_USER = issue.PK_USER,
                PK_WAREHOUSE = issue.PK_WAREHOUSE,
                NAMES = issue.NAMES,
                USER_NAME = issue.USER_NAME,
                STORE_NAME = issue.STORE_NAME
            });
        }

        public async Task<IEnumerable<DetailIssueReadDto>> ReadDetails(int id)
        {
            var details = await _goodsIssueRepository.ReadDetails(id);
            return details.Select(detail => new DetailIssueReadDto
            {
                CODE = detail.CODE,
                DESCRIPTION = detail.DESCRIPTION,
                MATERIAL = detail.MATERIAL,
                COLOUR = detail.COLOUR,
                BRAND_NAME = detail.BRAND_NAME,
                CATEGORY_NAME = detail.CATEGORY_NAME,
                QUANTITY = detail.QUANTITY,
                PRICE = detail.PRICE
            });
        }

        public async Task DisabledIssue(int id)
        {
            await _goodsIssueRepository.DisabledIssue(id);
        }
    }
}
