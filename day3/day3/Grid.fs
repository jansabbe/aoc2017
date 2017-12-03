module Grid

type Size = int
type Coordinate = int*int
type Grid = int [,]

let allPossibleSizes : Size seq =
    Seq.unfold (fun state -> Some(state, state + 2)) 1 // all sizes of grid should be odd

let maximumValueInGrid (size:Size) : int = pown size 2


let doesValueFitInGridWithSize (value:int) (size:Size) : bool =
    value <= (maximumValueInGrid size)

let neededSizeForValue (value : int) : Size =
    Seq.find (doesValueFitInGridWithSize value) allPossibleSizes

let sizeOfGrid (grid:Grid) : Size =
    Array2D.length1 grid

let growGrid (grid:Grid) : Grid =
    let smallSize = sizeOfGrid grid
    let biggerSize = smallSize + 2
    let biggerGrid : Grid = Array2D.zeroCreate biggerSize biggerSize
    Array2D.blit grid 0 0 biggerGrid 1 1 smallSize smallSize
    biggerGrid

let coordinatesOuterLayerFollowingSpiral (size:Size) : Coordinate list =
    let maxCoordinateInGrid : int = size-1
    let bottomToLeft = [maxCoordinateInGrid .. -1 .. 0] |> List.map (fun x -> x,maxCoordinateInGrid)
    let leftToTop = [(maxCoordinateInGrid-1) .. -1 .. 0] |> List.map (fun y -> 0,y)
    let topToRight = [1 .. maxCoordinateInGrid] |> List.map (fun x -> x,0)
    let rightToBottom = [1..(maxCoordinateInGrid-1)] |> List.map (fun y -> maxCoordinateInGrid, y)
    (bottomToLeft @ leftToTop @ topToRight @ rightToBottom) |> List.rev

let valuesOuterLayerFollowingSpiral (size:Size) : int list =
    let maxValueInOuterLayer = maximumValueInGrid size
    let minValueInOuterLayer = maximumValueInGrid (size - 2) + 1
    [minValueInOuterLayer..maxValueInOuterLayer]

let fillInOuterLayer (grid:Grid) : Grid =
    let size = sizeOfGrid grid
    let result = Array2D.copy grid
    let setValueAtCoordinate ((x,y):Coordinate) (value:int) =
        Array2D.set result y x value
    List.iter2 setValueAtCoordinate (coordinatesOuterLayerFollowingSpiral size) (valuesOuterLayerFollowingSpiral size)
    result

let rec generateGridForSize : Size -> Grid = function
    | 1 -> array2D [[1]]
    | n -> generateGridForSize (n - 2)
        |> growGrid
        |> fillInOuterLayer

let generateGridWithValue (value:int) : Grid =
    value
        |> neededSizeForValue
        |> generateGridForSize

let coordinatesForValue (value:int) (grid:Grid) =
    let result = ref (-1, -1)
    Array2D.iteri (fun y x v -> if v = value then result := (x,y)) grid
    !result

let distance (value:int) : int =
    let grid = generateGridWithValue value
    let (centerX, centerY) = coordinatesForValue 1 grid
    let (valueX, valueY) = coordinatesForValue value grid
    (abs (valueX - centerX)) + (abs (valueY - centerY))