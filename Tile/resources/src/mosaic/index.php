<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Tiles</title>
    <style>
        /* Grid display */
        .wrapper {
            display: grid;
            grid-template-columns: 64px 64px 64px 64px 64px 64px 64px 64px 64px 64px 64px 64px 64px 64px;
            grid-gap: 6px;
        }
        .tile {
            background-position: 50% 50%;
            background-repeat: no-repeat;
            background-size: cover;
            width: 64px;
            height: 64px;
        }
    </style>
</head>
<body>
    <div class="wrapper">
        <?php
        // Collect images names in an array
        $tiles = array();
        if ($handle = opendir('./tiles')) {
            while (false !== ($entry = readdir($handle))) {
                if ($entry != "." && $entry != ".." && fnmatch("*.png", $entry)) {
                    array_push($tiles, $entry);
                }
            }
            closedir($handle);
        }
        // Random order
        shuffle($tiles);
        // Display each tile in a grid
        foreach ($tiles as $tile) {
            ?>
            <div class="tile" style="background-image: url('<?= "tiles/$tile" ?>');"></div>
            <?php
        }
        ?>
    </div>
</body>
</html>