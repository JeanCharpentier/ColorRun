<html>
    <head>

    </head>
    <body>
        <?php
        include 'credentials.php';

        $sqlco = new mysqli($host,$user,$pwd,$db);

        $player = strip_tags($_POST['player']);

        if($sqlco->connect_error) {
            die("Connection failed: ".$sqlco->connect_error);
        }

        $query = "SELECT Player, HighScore FROM lb_normal WHERE 1 ORDER BY HighScore DESC;";

        $res = $sqlco->query($query);
        ?>
        <table>
            <tr>
                <td>Player name</td>
                <td>Score</td>
            </tr>
            <?php
            while($row = mysqli_fetch_row($res)) {
                echo "<tr><td>".$row[0]."</td><td>".$row[1]."</td></tr>";
            }
    
            if($player != "") {
                
            }else {
                
            }
            ?>
        </table>
        <?php
        $sqlco->close();
        ?>
    </body>
</html>