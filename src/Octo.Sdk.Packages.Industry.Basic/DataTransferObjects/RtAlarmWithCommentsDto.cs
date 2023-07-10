using Meshmakers.Octo.Sdk.ServiceClient.AssetRepositoryServices.Tenants;
using Newtonsoft.Json;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtAlarmWithCommentsDto : QlRtEntityDtoWithAssociations
{
    [QlConnection("children", "meshmakersEventsCommentConnection")]
    public QlItemsContainer<RtEventCommentDto>? AlarmComments { get; set; }
}