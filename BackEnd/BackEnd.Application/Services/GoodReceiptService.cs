using BackEnd.Application.Dtos.DetailsReceiptDtos;
using BackEnd.Application.Dtos.GoodsReceiptDtos;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;

namespace BackEnd.Application.Services
{
    public class GoodReceiptService
    {
        private readonly IGoodsReceiptRepository _goodsReceiptRepository;

        public GoodReceiptService(IGoodsReceiptRepository goodsReceiptRepository)
        {
            _goodsReceiptRepository = goodsReceiptRepository;
        }

        public async Task<string> CreateReceipt(GoodReceiptCreateDto createDto)
        {
            var receipt = new GoodsReceipt
            {
                DATE_PURCHASE = createDto.DATE_PURCHASE,
                TYPE_RECEIPT = createDto.TYPE_RECEIPT,
                TYPE_DOCUMENT = createDto.TYPE_DOCUMENT,
                DOCUMENT_NUMBER = createDto.DOCUMENT_NUMBER,
                ANNOTATIONS = createDto.ANNOTATIONS,
                PK_SUPPLIER = createDto.PK_SUPPLIER,
                PK_USER = createDto.PK_USER,
                PK_WAREHOUSE = createDto.PK_WAREHOUSE,
                details = createDto.details?.Select(d => new DetailsReceipt
                {
                    PK_PRODUCT = d.PK_PRODUCT,
                    QUANTITY = d.QUANTITY,
                    COST = d.COST
                }).ToList()
            };

            return await _goodsReceiptRepository.CreateReceipt(receipt);
        }

        public async Task<IEnumerable<GoodReceiptReadDto>> ReadReceipts(int id)
        {
            var receipts = await _goodsReceiptRepository.ReadReceipts(id);
            return receipts.Select(receipt => new GoodReceiptReadDto
            {
                PK_RECEIPT = receipt.PK_RECEIPT,
                CODE = receipt.CODE,
                DATE_PURCHASE = receipt.DATE_PURCHASE.HasValue ? receipt.DATE_PURCHASE.Value.ToString("dd/MM/yyyy") : null,
                DATE_REGISTRATION = receipt.DATE_REGISTRATION.HasValue ? receipt.DATE_REGISTRATION.Value.ToString("dd/MM/yyyy HH:mm") : null,
                TYPE_RECEIPT = receipt.TYPE_RECEIPT,
                TYPE_DOCUMENT = receipt.TYPE_DOCUMENT,
                DOCUMENT_NUMBER = receipt.DOCUMENT_NUMBER,
                ANNOTATIONS = receipt.ANNOTATIONS,
                STATUS = receipt.STATUS,
                PK_SUPPLIER = receipt.PK_SUPPLIER,
                PK_USER = receipt.PK_USER,
                PK_WAREHOUSE = receipt.PK_WAREHOUSE,
                COMPANY_NAME = receipt.COMPANY_NAME,
                USER_NAME = receipt.USER_NAME,
                STORE_NAME = receipt.STORE_NAME
            });
        }

        public async Task<IEnumerable<GoodReceiptReadDto>> SearchReceipt(string text, int id)
        {
            var receipts = await _goodsReceiptRepository.SearchReceipt(text, id);
            return receipts.Select(receipt => new GoodReceiptReadDto
            {
                PK_RECEIPT = receipt.PK_RECEIPT,
                CODE = receipt.CODE,
                DATE_PURCHASE = receipt.DATE_PURCHASE.HasValue ? receipt.DATE_PURCHASE.Value.ToString("dd/MM/yyyy") : null,
                DATE_REGISTRATION = receipt.DATE_REGISTRATION.HasValue ? receipt.DATE_REGISTRATION.Value.ToString("dd/MM/yyyy HH:mm") : null,
                TYPE_RECEIPT = receipt.TYPE_RECEIPT,
                TYPE_DOCUMENT = receipt.TYPE_DOCUMENT,
                DOCUMENT_NUMBER = receipt.DOCUMENT_NUMBER,
                ANNOTATIONS = receipt.ANNOTATIONS,
                STATUS = receipt.STATUS,
                PK_SUPPLIER = receipt.PK_SUPPLIER,
                PK_USER = receipt.PK_USER,
                PK_WAREHOUSE = receipt.PK_WAREHOUSE,
                COMPANY_NAME = receipt.COMPANY_NAME,
                USER_NAME = receipt.USER_NAME,
                STORE_NAME = receipt.STORE_NAME
            });
        }

        public async Task<IEnumerable<DetailReceiptReadDto>> ReadDetails(int id)
        {
            var details = await _goodsReceiptRepository.ReadDetails(id);
            return details.Select(detail => new DetailReceiptReadDto
            {
                CODE = detail.CODE,
                DESCRIPTION = detail.DESCRIPTION,
                MATERIAL = detail.MATERIAL,
                COLOUR = detail.COLOUR,
                BRAND_NAME = detail.BRAND_NAME,
                CATEGORY_NAME = detail.CATEGORY_NAME,
                QUANTITY = detail.QUANTITY,
                COST = detail.COST
            });
        }

        public async Task DisabledReceipt(int id)
        {
            await _goodsReceiptRepository.DisabledReceipt(id);
        }
    }
}
