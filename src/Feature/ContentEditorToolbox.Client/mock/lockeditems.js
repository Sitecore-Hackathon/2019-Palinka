module.exports = {
    'GET /sitecore/api/ssc/Feature-ContentEditorToolbox-Controllers/EditorToolbox/-/GetMyLockedItems': function (req, res) {
      res.json([{
        
        "Id":"{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}",
        "Path":"/sitecore/content/home",
        "Name":"TestItem",
        "IconPath":"/-/icon/Applications/48x48/photo_portrait.png",
        "Languages":[{"Name":"France","Code":"fr-CA"}, {"Name":"English", "Code":"en-GB"}],
        "HasPresentation":true,
        "IsPublished":false,
        "WorkflowState": "",
        "TemplateName":"SampleTemplate",
        "WorkflowState":"Sample State"
      },
      {
        
        "ItemId":"A6599689-3616-4938-A5BB-9EC65480D2F3",
        "ItemPath":"/sitecore/content/home",
        "ItemName":"TestItem2",
        "Icon":"/-/icon/Applications/48x48/photo_portrait.png",
        "HasPresentation":false,
        "IsPublished":true,
        "WorkflowState": "",
        "TemplateName":"SampleTemplate",
        "WorkflowState":"Sample State"
      }]);
    }
  };
  