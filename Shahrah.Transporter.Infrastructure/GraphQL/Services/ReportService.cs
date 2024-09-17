using GraphQL;
using GraphQL.Client.Abstractions;
using Shahrah.Transporter.Domain.GraphQL;
using Shahrah.Transporter.Domain.GraphQL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shahrah.Transporter.Infrastructure.GraphQL.Services;

public class ReportService : IReportService
{
    private readonly IGraphQLClient _client;

    public ReportService(IGraphQLClient client)
    {
        _client = client;
    }


    public async Task<List<ReportOrder>> GetOrderItems(Guid[] guids)
    {
        var query = new GraphQLRequest
        {
            Query = @"
                query($identities: String!) {
                  orders(identities: $identities) {
                    id
                    senderName
                    senderMobileNumber
                    loadReceiverFirstName
                    loadReceiverLastName 
                    loadReceiverMobileNumber
                    source {
                        cityId
                        cityName
                        provinceId
                        provinceName
                        latitude
                        longitude
                    }
                    destination {
                        cityId
                        cityName
                        provinceId
                        provinceName
                        latitude
                        longitude
                    }
                    items{
                      id
                      driver{
                        id
                        firstName
                        lastName
                        nationalCode
                        birthDate
                        mobileNumber
                        drivingLicenseNumber
                        drivingLicenseExpirationDate
                      }
                    }
                  }
                }
        ",
            Variables = new { identities = string.Join('|', guids) }
        };

        var response = await _client.SendQueryAsync<ReportOrderListModel>(query);
        return response.Data.Orders;
    } 
    public async Task<List<ReportOrder>> GetTripEndedReportData(Guid[] guids)
    {
        var query = new GraphQLRequest
        {
            Query = @"
                    query($identities: String!) {
                      orders(identities: $identities) {
                        id
                        senderName
                        senderMobileNumber
                        source {
                          cityId
                          cityName
                          provinceId
                          provinceName
                          latitude
                          longitude
                        }
                        destination {
                          cityId
                          cityName
                          provinceId
                          provinceName
                          latitude
                          longitude
                        }
                        items {
                          id
                          endTripDateTime
                          driver {
                            firstName
                            lastName
                            mobileNumber
                          }
                        }
                      }
                    }
        ",
            Variables = new { identities = string.Join('|', guids) }
        };

        var response = await _client.SendQueryAsync<ReportOrderListModel>(query);
        return response.Data.Orders;
    }

    public async Task<List<ReportOrder>> PaidOrderItemReportData(Guid[] guids)
    {
        var query = new GraphQLRequest
        {
            Query = @"
                    query($identities: String!) {
                      orders(identities: $identities) {
                        id
                        items {
                          id
                          driver {
                            firstName
                            lastName
                            mobileNumber
                          }
                        }
                      }
                    }
        ",
            Variables = new { identities = string.Join('|', guids) }
        };

        var response = await _client.SendQueryAsync<ReportOrderListModel>(query);
        return response.Data.Orders;
    }
}