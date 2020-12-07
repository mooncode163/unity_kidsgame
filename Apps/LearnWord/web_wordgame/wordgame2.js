var writer;
var isCharVisible;
var isOutlineVisible;
var countStroke = 0;
var countPoint = 0;
var word_size = 512



// let text = load("test.txt");
// console.log(text);
function LoadFile(name) {
	let xhr = new XMLHttpRequest(),
		okStatus = document.location.protocol === "file:" ? 0 : 200;
	xhr.open('GET', name, false);
	xhr.overrideMimeType("text/html;charset=utf-8");//默认为utf-8
	xhr.send(null);
	return xhr.status === okStatus ? xhr.responseText : null;
}

function GetCountStroke() {
	return countStroke
}

function GetCountPoint() {
	return countPoint
}
// SVG画在直线中间的箭头 https://blog.csdn.net/tuposky/article/details/40677477
// https://hanziwriter.org/docs.html
// https://cdn.jsdelivr.net/npm/hanzi-writer-data@2.0/%E4%BB%96.json
function printStrokePoints(data) {
	var pointStrs = data.drawnPath.points.map(point => `{x: ${point.x}, y: ${point.y}}`);
	console.log(`[${pointStrs.join(', ')}]`);
}

function updateCharacter(word) {
	// document.querySelector('#target').innerHTML = '';

	// var character = document.querySelector('.js-char').value;
	//window.location.hash = character;
	writer = HanziWriter.create('target', word, {
		width: word_size,
		height: word_size,
		// radicalColor: '#166E16',
		onCorrectStroke: printStrokePoints,
		onMistake: printStrokePoints,
		showCharacter: false,
	});

	// var writer = HanziWriter.create('character-target-div', '国', {
	// 	width: 100,
	// 	height: 100,
	// 	padding: 5,
	// 	showOutline: true
	//   });    
	isCharVisible = true;
	isOutlineVisible = true;
	window.writer = writer;
}

// http://www.runoob.com/index.php?id=1&image=awesome.jpg
function getQueryVariable(variable) {
	console.log(window.location.href);
	var query = window.location.search.substring(1);
	var vars = query.split("&");
	for (var i = 0; i < vars.length; i++) {
		var pair = vars[i].split("=");
		if (pair[0] == variable) { return pair[1]; }
	}
	return "";
}

function renderFanningStrokes(target, strokes) {
	var svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
	svg.style.width = word_size.toString();
	svg.style.height = word_size.toString();
	svg.style.backgroundColor = "white";
	// svg.style.border = '1px solid #EEE'
	// svg.style.marginRight = '3px'
	target.appendChild(svg);
	var group = document.createElementNS('http://www.w3.org/2000/svg', 'g');

	// set the transform property on the g element so the character renders at 75x75
	var transformData = HanziWriter.getScalingTransform(word_size, word_size);
	group.setAttributeNS(null, 'transform', transformData.transform);
	svg.appendChild(group);

	strokes.forEach(function (strokePath) {
		var path = document.createElementNS('http://www.w3.org/2000/svg', 'path');
		path.setAttributeNS(null, 'd', strokePath);
		// style the character paths
		//   path.style.fill = '#555';
		group.appendChild(path);
	});
}



// 箭头
function GetPathArrow(x1, y1, x2, y2) {
	var path;
	var slopy, cosy, siny;

	// 越大 箭头越长
	var Par = 30.0;
	// flag 越大 夹角越小
	var flag = 3

	var x3, y3;
	slopy = Math.atan2((y1 - y2), (x1 - x2));
	cosy = Math.cos(slopy);
	siny = Math.sin(slopy);

	path = "M" + x1 + "," + y1 + " L" + x2 + "," + y2;

	x3 = ((x1) + (x2)) / 2;
	y3 = ((y1) + (y2)) / 2;

	x3 = x2;
	y3 = y2;



	path += " M" + x3 + "," + y3;

	path += " L" + ((x3) + (Par * cosy - (Par / flag) * siny)) + "," + ((y3) + (Par * siny + (Par / flag) * cosy));


	path += " M" + ((x3) + (Par * cosy + (Par / flag) * siny) + "," + ((y3) - ((Par / flag) * cosy - Par * siny)));
	path += " L" + x3 + "," + y3;


	return path;
}

function CreateDefs(svg, strokes) {
	var defs = document.createElementNS('http://www.w3.org/2000/svg', 'defs');
	svg.appendChild(defs);
	console.log("CreateDefs")
	// <clipPath id="mask-1">
	//                     <path d="M 250 530 Q 287 585 330 658 Q 354 707 377 731 Q 386 743 381 756 Q 377 769 346 791 Q 318 809 299 808 Q 280 805 289 783 Q 305 752 293 724 Q 236 568 86 372 Q 76 362 74 354 Q 71 344 85 346 Q 118 355 224 495 L 250 530 Z"></path>
	//             </clipPath>

	var idx = 0
	strokes.forEach(function (strokePath) {
		var clipPath = document.createElementNS('http://www.w3.org/2000/svg', 'clipPath');
		clipPath.setAttributeNS(null, 'id', "mask-" + idx);
		var path = document.createElementNS('http://www.w3.org/2000/svg', 'path');
		path.setAttributeNS(null, 'd', strokePath);

		clipPath.appendChild(path);
		defs.appendChild(clipPath);
		idx = idx + 1
	});


}

// index 为笔画
function renderAnimateStrokes(target, strokes, jsonStrokes, index) {
	var svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
	svg.style.width = word_size.toString();
	svg.style.height = word_size.toString();
	svg.style.backgroundColor = "white";
	// svg.style.border = '1px solid #EEE'
	// svg.style.marginRight = '3px'
	target.appendChild(svg);
	CreateDefs(svg, jsonStrokes)

	var group = document.createElementNS('http://www.w3.org/2000/svg', 'g');

	// set the transform property on the g element so the character renders at 75x75
	var transformData = HanziWriter.getScalingTransform(word_size, word_size);
	group.setAttributeNS(null, 'transform', transformData.transform);
	svg.appendChild(group);

	strokes.forEach(function (strokePath) {
		var path = document.createElementNS('http://www.w3.org/2000/svg', 'path');
		var idx = index
		mask = "url(#mask-" + idx.toString() + ")"
		path.setAttributeNS(null, 'clip-path', mask);
		path.setAttributeNS(null, 'd', strokePath);
		path.setAttributeNS(null, 'stroke', "red");
		path.setAttributeNS(null, 'stroke-width', "100");
		path.setAttributeNS(null, 'fill', "none");
		path.setAttributeNS(null, 'stroke-linecap', "round");
		path.setAttributeNS(null, 'stroke-linejoin', "miter");


		// clip-path="url(#mask-1)"

		// style the character paths
		//   path.style.fill = '#555';
		group.appendChild(path);
	});


}

function GetStrokePointCount(jsonData, index) {
	return jsonData.medians[index].length
}

function GetStrokePoint(jsonData, indexStroke, index) {
	var list = jsonData.medians[indexStroke][index]
	var x = 0, y = 0;
	for (var k = 0; k < list.length; k++) {
		var v = list[k]
		if (k == 0) {
			x = v;
		}
		if (k == 1) {
			y = v;
		}
	}

	console.log(x)
	console.log(y)
	return { x, y };
}

// 不包含indexEnd
// var strpath = "M 300 793  L 317 780   L 338 747  L 308 679 L 201 496  L 136 410 L 83 355 "
function GetStrokePointPath(jsonData, indexStroke, indexStart, indexEnd) {
	var list = jsonData.medians[indexStroke]
	var strpath = ""
	for (var j = 0; j < list.length; j++) {
		var list1 = list[j]
		if (j >= indexEnd) {
			break
		}
		if (j == indexStart) {
			strpath = "M "
		} else if (j > indexStart) {
			strpath += "L "
		}
		for (var k = 0; k < list1.length; k++) {
			var x = list1[k]
			// console.log(x)
			strpath += x.toString() + " "
		}
	}
	// console.log(strpath)
	// strokes.push(strpath)
	return strpath
}



function GetMask(index) {
	return "url(#mask-" + index.toString() + ")"
}

// index 为笔画 strokes为point
function renderStrokesWithCircle(target, strokes, jsonStrokes, index, cx, cy, cr) {
	var svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
	svg.style.width = word_size.toString();
	svg.style.height = word_size.toString();
	svg.style.backgroundColor = "white";
	// svg.style.border = '1px solid #EEE'
	// svg.style.marginRight = '3px'
	target.appendChild(svg);
	CreateDefs(svg, jsonStrokes)

	var group = document.createElementNS('http://www.w3.org/2000/svg', 'g');

	// set the transform property on the g element so the character renders at 75x75
	var transformData = HanziWriter.getScalingTransform(word_size, word_size);
	group.setAttributeNS(null, 'transform', transformData.transform);
	svg.appendChild(group);

	strokes.forEach(function (strokePath) {
		var path = document.createElementNS('http://www.w3.org/2000/svg', 'path');
		mask = GetMask(index)
		path.setAttributeNS(null, 'clip-path', mask);
		path.setAttributeNS(null, 'd', strokePath);
		path.setAttributeNS(null, 'stroke', "black");
		path.setAttributeNS(null, 'stroke-width', "100");
		path.setAttributeNS(null, 'fill', "none");
		path.setAttributeNS(null, 'stroke-linecap', "round");
		path.setAttributeNS(null, 'stroke-linejoin', "miter");
		group.appendChild(path);
	});
	CreateCircle(group, index, cx, cy, cr)

}

// 画圆 <circle cx="136" cy="410" r="8" fill="red"/>
//  index 为笔画 
function CreateCircle(parent, index, x, y, r) {
	mask = GetMask(index)
	var circle = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
	circle.setAttributeNS(null, 'cx', x);
	circle.setAttributeNS(null, 'cy', y);
	circle.setAttributeNS(null, 'r', r);
	circle.setAttributeNS(null, 'fill', "red");
	circle.setAttributeNS(null, 'clip-path', mask);
	parent.appendChild(circle);

}
// file:///F:/sourcecode/unity/product/kidsgame/kidsgameUnityNow/Assets/Script/Apps/LearnWord/web_wordgame/index2.html?type=circle&word=他&index=0&x=300&y=793
function OnTypeCircle(word, index) {
	var cx = getQueryVariable("x")
	var cy = getQueryVariable("y")
	var cr = "8"

	// charData json
	HanziWriter.loadCharacterData(word).then(function (charData) {
		var target = document.getElementById('target');
		count = charData.strokes.length
		jsonPoints = charData.medians
		// "M 250 530 Q 287
		// var strpath = "M 300 793  L 317 780   L 338 747  L 308 679 L 201 496  L 136 410 L 83 355 "
		var strpath = ""
		var strokes = []

		var i = index
		var count = GetStrokePointCount(charData, i)
		countPoint = count
		var end = GetStrokePointCount(charData, i)
		strpath = GetStrokePointPath(charData, i, 0, end)
		console.log("strpath=", strpath)
		strokes.push(strpath)
		count = strokes.length
		var strokesPortion = strokes.slice(0, count + 1);
		renderStrokesWithCircle(target, strokesPortion, charData.strokes, index, cx, cy, cr);

	});
}

// file:///F:/sourcecode/unity/product/kidsgame/kidsgameUnityNow/Assets/Script/Apps/LearnWord/web_wordgame/index.html?type=animate&word=他&index=0&end_index_point=2
function OnTypeAnimate(word, index) {
	var str = getQueryVariable("end_index_point")
	if (str == "") {
		str = "2";
	}
	var end_index_point = parseInt(str);

	// charData json
	HanziWriter.loadCharacterData(word).then(function (charData) {
		var target = document.getElementById('target');
		count = charData.strokes.length
		jsonPoints = charData.medians
		point0 = jsonPoints[0]
		// "M 250 530 Q 287
		// var strpath = "M 300 793  L 317 780   L 338 747  L 308 679 L 201 496  L 136 410 L 83 355 "
		var strpath = ""
		var strokes = []

		var i = index
		var count = GetStrokePointCount(charData, i)
		countPoint = count
		if (end_index_point < 0) {
			return
		}
		if (end_index_point > count) {
			return
		}

		// for (var i = 0; i < jsonPoints.length; i++) 
		{

			var list = jsonPoints[i]
			var end = GetStrokePointCount(charData, i)
			// count=list.length
			end = end_index_point
			strpath = GetStrokePointPath(charData, i, 0, end)
			// for (var j = 0; j < list.length; j++) {
			// 	var list1 = list[j]
			// 	if (j == 0) {
			// 		strpath = "M "
			// 	} else {
			// 		strpath += "L "
			// 	}
			// 	for (var k = 0; k < list1.length; k++) {
			// 		var x = list1[k]
			// 		console.log(x)
			// 		strpath += x.toString() + " "
			// 	}
			// }
			console.log("strpath=", strpath)
			strokes.push(strpath)
		}

		// var strokes = charData.strokes
		// strokes = strokes.concat(charData.strokes)

		count = strokes.length

		// console.log(jsonPoints)
		{
			var strokesPortion = strokes.slice(0, count + 1);
			renderAnimateStrokes(target, strokesPortion, charData.strokes, index);
		}
	});
}




// index 为笔画
function renderStrokeLineShow(target, strokes, jsonStrokes, listCenter) {
	var svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
	svg.style.width = word_size.toString();
	svg.style.height = word_size.toString();
	svg.style.backgroundColor = "white";
	// svg.style.border = '1px solid #EEE'
	// svg.style.marginRight = '3px'
	target.appendChild(svg);
	CreateDefs(svg, jsonStrokes)

	var group = document.createElementNS('http://www.w3.org/2000/svg', 'g');

	// set the transform property on the g element so the character renders at 75x75
	var transformData = HanziWriter.getScalingTransform(word_size, word_size);
	group.setAttributeNS(null, 'transform', transformData.transform);
	svg.appendChild(group); 
	var color = "rgb(32, 173, 222,255)"
	var idx = 0;
	strokes.forEach(function (strokePath) {
		var path = document.createElementNS('http://www.w3.org/2000/svg', 'path');
		// var idx = index
		// mask = "url(#mask-" + idx.toString() + ")"
		// path.setAttributeNS(null, 'clip-path', mask);
		path.setAttributeNS(null, 'd', strokePath);
		path.setAttributeNS(null, 'stroke', color);
		path.setAttributeNS(null, 'stroke-width', "5");
		path.setAttributeNS(null, 'fill', "none");
		path.setAttributeNS(null, 'stroke-linecap', "round");
		path.setAttributeNS(null, 'stroke-linejoin', "miter");


		// clip-path="url(#mask-1)"

		// style the character paths
		//   path.style.fill = '#555';
		group.appendChild(path);


		r = 32
		pt = listCenter[idx]
		var circle = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
		circle.setAttributeNS(null, 'cx', pt.x);
		circle.setAttributeNS(null, 'cy', pt.y);
		circle.setAttributeNS(null, 'r', r);
		circle.setAttributeNS(null, 'fill', color);
		group.appendChild(circle);


		// <text x="220" y="853.7" fill="red">I love SVG</text>

		var text = document.createElementNS('http://www.w3.org/2000/svg', 'text');
		text.setAttributeNS(null, 'x', pt.x);
		text.setAttributeNS(null, 'y', pt.y);
		text.setAttributeNS(null, 'fill', "white");
		text.setAttributeNS(null, 'font-size', "30");
		text.setAttributeNS(null, 'alignment-baseline', "middle");
		text.setAttributeNS(null, 'text-anchor', "middle"); 
		// 

		// "scale(1, -1) translate(0, -1586)"
		// 文字倒转显示 调整
		var ofty = -pt.y*2;
		var str = `scale(1, -1) translate(0, ${ofty})`;
		text.setAttributeNS(null, 'transform', str);  

		text.innerHTML = (idx + 1).toString();
		group.appendChild(text);

		idx++;
	});

 

}


//file:///F:/sourcecode/unity/product/kidsgame/kidsgameUnityNow/Assets/Script/Apps/LearnWord/web_wordgame/index2.html?type=stroke_lineshow&word=他
function OnTypeStrokeLineShow(word) {

	// charData json
	HanziWriter.loadCharacterData(word).then(function (charData) {
		var target = document.getElementById('target');
		count = charData.strokes.length
		// "M 250 530 Q 287
		// var strpath = "M 300 793  L 317 780   L 338 747  L 308 679 L 201 496  L 136 410 L 83 355 "

		var strokes = []
		var listCenter = []
		for (var i = 0; i < charData.strokes.length; i++) {
			var strpath = ""
			jsonPoints = charData.medians[i]
			var end = GetStrokePointCount(charData, i)
			strpath = GetStrokePointPath(charData, i, 0, end)
			pt1 = GetStrokePoint(charData, i, jsonPoints.length - 2)
			pt2 = GetStrokePoint(charData, i, jsonPoints.length - 1)
			pathArrow = GetPathArrow(pt1.x, pt1.y, pt2.x, pt2.y)
			strpath += pathArrow
			console.log("strpath=", strpath)
			strokes.push(strpath)

			pt = GetStrokePoint(charData, i, 0)
			listCenter.push(pt)

		}
		count = strokes.length
		var strokesPortion = strokes.slice(0, count + 1);
		renderStrokeLineShow(target, strokesPortion, charData.strokes, listCenter);

	});
}



window.onload = function () {
	// var char = decodeURIComponent(window.location.hash.slice(1));
	// if (char) {
	// 	document.querySelector('.js-char').value = char;
	// }
	var word = getQueryVariable("word")
	word = decodeURI(word);
	if (word == "") {
		word = "他";
	}

	var index = getQueryVariable("index")
	if (index == "") {
		index = "0";
	}
	var intvalue = parseInt(index);
	// word = "他";
	console.log("word=", word);
	// document.querySelector('.js-char').value=word;

	var type = getQueryVariable("type")
	if (type == "animate") {
		OnTypeAnimate(word, intvalue)
		return
	}
	if (type == "circle") {
		OnTypeCircle(word, intvalue)
		return
	}

	if (type == "stroke_lineshow") {
		OnTypeStrokeLineShow(word)
		return
	}

	// 显示完整的汉字
	if (intvalue < 0) {
		// updateCharacter(word);
		HanziWriter.loadCharacterData(word).then(function (charData) {
			var target = document.getElementById('target');
			countStroke = charData.strokes.length
			jsonPoints = charData.medians
			console.log(jsonPoints)
			{
				var strokesPortion = charData.strokes.slice(0, countStroke + 1);
				renderFanningStrokes(target, strokesPortion);
			}
		});
	}
	// writer.updateColor('strokeColor', 'rgba(255, 0, 0, 1)');
	// writer.updateColor('highlightColor', 'rgba(255, 0, 0, 1)');
	// writer.updateColor('drawingColor', 'rgba(255, 0, 0, 1)');



	// writer.animateCharacter( );

	// document.querySelector('.js-char-form').addEventListener('submit', function (evt) {
	// 	evt.preventDefault();
	// 	updateCharacter();
	// });

	// document.querySelector('.js-toggle').addEventListener('click', function () {
	// 	isCharVisible ? writer.hideCharacter() : writer.showCharacter();
	// 	isCharVisible = !isCharVisible;
	// });
	// document.querySelector('.js-toggle-hint').addEventListener('click', function () {
	// 	isOutlineVisible ? writer.hideOutline() : writer.showOutline();
	// 	isOutlineVisible = !isOutlineVisible;
	// });
	// document.querySelector('.js-animate').addEventListener('click', function () {
	// 	writer.animateCharacter();
	// });
	// document.querySelector('.js-quiz').addEventListener('click', function () {
	// 	writer.quiz({
	// 		showOutline: true
	// 	});
	// });
	var div = document.getElementById('target');
	var svg = document.getElementsByTagName('svg');
	// html2canvas(div).then(function(canvas) {
	// 	var dataUrl = canvas.toDataURL();
	// 	var newImg = document.createElement("img");
	// 	newImg.src =  dataUrl;
	// 	document.body.appendChild(newImg);
	// });   
	// svg.style.backgroundColor = "red";

	// saveSvgAsPng(document.getElementById("target"), "diagram.png", 3);


	// 显示单一的笔画
	if (intvalue >= 0) {
		HanziWriter.loadCharacterData(word).then(function (charData) {
			var target = document.getElementById('target');
			countStroke = charData.strokes.length
			var i = intvalue
			// for (var i = 0; i < charData.strokes.length; i++) 
			{
				var strokesPortion = charData.strokes.slice(i, i + 1);
				renderFanningStrokes(target, strokesPortion);
			}
		});
	}
}
