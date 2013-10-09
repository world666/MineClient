var imgArrowRight = new Image();
imgArrowRight.src = "../../Images/arrow_right.png";
var imgArrowLeft = new Image();
imgArrowLeft.src = "../../Images/arrow_left.png";
var imgArrowTop = new Image();
imgArrowTop.src = "../../Images/arrow_top.png";
var imgArrowBottom = new Image();
imgArrowLeft.src = "../../Images/arrow_bottom.png";

function DrawHorizontalPipe(x, y, width, height, side) {
    var c = document.getElementById("canVas");
    var ctx = c.getContext("2d");
    var grd = ctx.createLinearGradient(0, 0, 0, height);
    grd.addColorStop(0, "#898989");
    grd.addColorStop(0.5, "white");
    grd.addColorStop(1, "#898989");

    var start;
    var current = 0;
    var end;
    var dx;
    if (side == "right") {
        start = x - 20;
        end = x + width;
        dx = 1;
    } else if (side == "left") {
        start = x + width-10;
        end = x - 30;
        dx = -1;
    } else {
        dx = 0;
    }
    current = start;
    setInterval(function() {
        ctx.clearRect(x, y, width, height);
        ctx.fillStyle = grd;
        ctx.fillRect(x, y, width, height);
        if (side == "right") 
            ctx.drawImage(imgArrowRight, current += dx, y + height / 2 - 8);
        else 
            ctx.drawImage(imgArrowLeft, current += dx, y + height / 2 - 8);
        if (current == end)
            current = start;
    }, 10);
}

function DrawVerticalPipe(x, y, width, height, side) {
    var c = document.getElementById("canVas");
    var ctx = c.getContext("2d");
    var grd = ctx.createLinearGradient(0, 0, width, 0);
    grd.addColorStop(0, "#898989");
    grd.addColorStop(0.5, "white");
    grd.addColorStop(1, "#898989");

    var start;
    var current = 0;
    var end;
    var dy;
    if (side == "bottom") {
        start = y - 30;
        end = y + height;
        dy = 1;
    } else if (side == "top") {
        start = y + height;
        end = y - 30;
        dy = -1;
    } else {
        dy = 0;
    }
    current = start;
    setInterval(function() {
        ctx.clearRect(x, y, width, height);
        ctx.fillStyle = grd;
        ctx.fillRect(x, y, width, height);
        if (side=="bottom")
            ctx.drawImage(imgArrowBottom, x + width / 2 - 8, current += dy); // Рисуем изображение от точки с координатами 0, 0
        else
            ctx.drawImage(imgArrowTop, x + width / 2 - 8, current += dy);
        if (current == end)
            current = start;
    }, 10);
}

function DrawPipeBinderDouble(x, y, width, height) 
    {
        var c=document.getElementById("canVas");
        var ctx=c.getContext("2d");
        ctx.fillStyle="#B9B9B9";
        ctx.fillRect(x,y,width,height);
    }