window.rCaptcha = {
    draw: function (canvasRef, text, width, height) {
        // unwrap the ElementReference to real canvas element
        const canvas = canvasRef instanceof HTMLCanvasElement ? canvasRef : document.getElementById(canvasRef.id || canvasRef._blazor_id);
        if (!canvas) return;

        const ctx = canvas.getContext("2d");
        if (!ctx) return;

        const r = (min, max) => Math.floor(Math.random() * (max - min + 1) + min);

        ctx.fillStyle = `rgb(${r(70, 100)},${r(60, 80)},${r(50, 90)})`;
        ctx.fillRect(0, 0, width, height);

        const fonts = ["Courier", "Arial", "Verdana", "Times New Roman"];
        let x = 10;

        for (let i = 0; i < text.length; i++) {
            const ch = text[i];
            const angle = r(-15, 25);
            const font = fonts[r(0, fonts.length - 1)];
            const size = r(height / 2, height / 2 + height / 4);

            ctx.save();
            ctx.font = `${size}px ${font}`;
            ctx.fillStyle = `rgb(${r(100, 255)},${r(110, 255)},${r(90, 255)})`;
            ctx.translate(x, height / 1.4);
            ctx.rotate(angle * Math.PI / 180);
            ctx.fillText(ch, 0, 0);
            ctx.restore();

            x += size / 1.2;
        }

        ctx.strokeStyle = "rgba(0,0,0,0.3)";
        ctx.lineWidth = 1;

        ctx.beginPath();
        ctx.moveTo(0, r(0, height));
        ctx.lineTo(width, r(0, height));
        ctx.stroke();

        ctx.beginPath();
        ctx.moveTo(0, r(0, height));
        ctx.lineTo(width, r(0, height));
        ctx.stroke();

        ctx.beginPath();
        ctx.ellipse(r(-width, width), r(-height, height), width, height, 0, 0, Math.PI * 2);
        ctx.stroke();
    }
};
