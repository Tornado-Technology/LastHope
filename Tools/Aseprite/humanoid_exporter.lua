local function getAllLayers(layers, all)
    all = all == nil and {} or all

    for _, layer in ipairs(layers) do
        if layer.layers ~= nil then
            getAllLayers(layer.layers, all)
        else
            table.insert(all, layer)
        end
    end

    return all
end

local spr = app.sprite
if not spr then
    return print('No active sprite')
end

local path, title = spr.filenamematch(^(.+[])(.-).([^.])$)
local msg = { Do you want to exportoverwrite the following files }

local groups = {}
-- We get all layers except groups
local allLayers = getAllLayers(spr.layers)

-- Hide all layers
local hiddenLayers = {}
for _, layer in ipairs(allLayers) do
    if layer.isVisible then
        layer.isVisible = false
        table.insert(hiddenLayers, layer)
    end
end


for i, group in ipairs(spr.layers) do
    if group.isGroup then
        for i, layer in ipairs(group.layers) do
            layer.isVisible = true
            
            for i, tag in ipairs(spr.tags) do
                local fn = path .. '' .. group.name .. '' .. layer.name .. '' .. tag.name
                app.command.ExportSpriteSheet {
                    ui = false,
                    type = SpriteSheetType.HORIZONTAL,
                    textureFilename = fn .. '.png',
                    tag = tag.name,
                    listLayers = false,
                    listTags = false,
                    listSlices = false,
                }
            end

            layer.isVisible = false
        end
    end
end


-- Show all hidden layers
for _, layer in ipairs(hiddenLayers) do
    layer.isVisible = true
end
