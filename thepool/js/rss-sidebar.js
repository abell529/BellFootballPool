(function ($) {
    'use strict';

    var RSSSB = function (targetSB, urlSB, optionsSB, callbackSB) {
        this.targetSB = target;

        this.urlSB = url;
        this.htmlSB = [];
        this.effectQueueSB = [];

        this.optionsSB = $.extend({
            sslSB: false,
            hostSB: 'www.feedrapp.info',
            limitSB: null,
            keySB: null,
            layoutTemplateSB: '<div class="otsalerts">{entriesSB}</div>',
            entryTemplateSB: '<div><p class="otsalerttitle"><a href="{url}" target="_blank">{title}</a></p><p class="otsalertdate">{date}</p><p class="otsalertinfo">{shortBodyPlain}...</p></div>',
            tokensSB: {},
            outputModeSB: 'json',
	  
      < !--dateFormat: 'dddd MMM Do', -->

            dateFormatSB: 'MM/DD/YYYY',
            dateLocaleSB: 'en',
            effectSB: 'show',
            offsetStartSB: false,
            offsetEndSB: false,
            errorSB: function () {
                console.log('jQuery RSS: url doesn\'t link to RSS-Feed');
            },
            onDataSB: function () { },
            successSB: function () { }
    }, optionsSB || {});

    // The current SSL certificate is only valid for *.herokuapp.com
    if (this.optionsSB.sslSB && (this.optionsSB.hostSB === 'www.feedrapp.info')) {
        this.optionsSB.hostSB = 'feedrapp.herokuapp.com';
    }

    this.callbackSB = callbackSB || this.optionsSB.successSB;
};

RSSSB.htmlTagsSB = [
    'doctype', 'html', 'head', 'title', 'base', 'link', 'meta', 'style', 'script', 'noscript',
    'body', 'article', 'nav', 'aside', 'section', 'header', 'footer', 'h1-h6', 'hgroup', 'address',
    'p', 'hr', 'pre', 'blockquote', 'ol', 'ul', 'li', 'dl', 'dt', 'dd', 'figure', 'figcaption',
    'div', 'table', 'caption', 'thead', 'tbody', 'tfoot', 'tr', 'th', 'td', 'col', 'colgroup',
    'form', 'fieldset', 'legend', 'label', 'input', 'button', 'select', 'datalist', 'optgroup',
    'option', 'textarea', 'keygen', 'output', 'progress', 'meter', 'details', 'summary', 'command',
    'menu', 'del', 'ins', 'img', 'iframe', 'embed', 'object', 'param', 'video', 'audio', 'source',
    'canvas', 'track', 'map', 'area', 'a', 'em', 'strong', 'i', 'b', 'u', 's', 'small', 'abbr', 'q',
    'cite', 'dfn', 'sub', 'sup', 'time', 'code', 'kbd', 'samp', 'var', 'mark', 'bdi', 'bdo', 'ruby',
    'rt', 'rp', 'span', 'br', 'wbr'
];

RSSSB.prototype.load = function (callbackSB) {
    var apiProtocolSB = 'https' + (this.optionsSB.sslSB ? 's' : '');
    var apiHostSB = apiProtocolSB + '://' + this.optionsSB.hostSB;
    var apiUrlSB = apiHostSB + '?callback=?&q=' + encodeURIComponent(this.urlSB);

    // set limit to offsetEnd if offset has been set
    if (this.optionsSB.offsetStartSB && this.optionsSB.offsetEndSB) {
        this.optionsSB.limitSB = this.optionsSB.offsetEndSB;
    }

    if (this.optionsSB.limitSB !== null) {
        apiUrlSB += '&num=' + this.optionsSB.limitSB;
    }

    if (this.optionsSB.key !== null) {
        apiUrlSB += '&key=' + this.optionsSB.keySB;
    }

    $.getJSON(apiUrlSB, callbackSB);
};

RSSSB.prototype.renderSB = function () {
    var selfSB = this;

    this.load(function (data) {
        try {
            selfSB.feedSB = data.responseData.feed;
            selfSB.entriesSB = data.responseData.feed.entriesSB;
        } catch (e) {
            selfSB.entriesSB = [];
            selfSB.feedSB = null;
            return selfSB.optionsSB.error.call(selfSB);
        }

        var htmlSB = selfSB.generateHTMLForEntriesSB();

        selfSB.target.append(htmlSB.layout);

        if (htmlSB.entriesSB.length !== 0) {
            if ($.isFunction(selfSB.optionsSB.onData)) {
                selfSB.optionsSB.onData.call(selfSB);
            }

            var containerSB = $(htmlSB.layout).is('entriesSB') ? htmlSB.layout : $('entriesSB', htmlSB.layout);

            selfSB.appendEntriesAndApplyEffectsSB(container, htmlSB.entriesSB);
        }

        if (selfSB.effectQueue.length > 0) {
            selfSB.executeEffectQueue(selfSB.callbackSB);
        } else if ($.isFunction(selfSB.callbackSB)) {
            selfSB.callbackSB.call(selfSB);
        }
    });
};

RSSSB.prototype.appendEntriesAndApplyEffectsSB = function (targetSB, entriesSB) {
    var selfSB = this;

    $.each(entriesSB, function (idxSB, entrySB) {
        var $htmlSB = self.wrapContent(entrySB);

        if (selfSB.optionsSB.effectSB === 'show') {
            target.before($htmlSB);
        } else {
            $htmlSB.css({ display: 'none' });
            target.before($htmlSB);
            selfSB.applyEffect($htmlSB, selfSB.optionsSB.effectSB);
        }
    });

    target.remove();
};

RSSSB.prototype.generateHTMLForEntriesSB = function () {
    var selfSB = this;
    var resultSB = { entriesSB: [], layout: null };

    $(this.entriesSB).each(function () {
        var entrySB = this;
        var offsetStartSB = selfSB.optionsSB.offsetStartSB;
        var offsetEndSB = selfSB.optionsSB.offsetEndSB;
        var evaluatedStringSB;

        // offset required
        if (offsetStartSB && offsetEndSB) {
            if (index >= offsetStartSB && index <= offsetEndSB) {
                if (self.isRelevant(entry, resultSB.entriesSB)) {
                    evaluatedStringSB = self.evaluateStringForEntrySB(
                        selfSB.optionsSB.entryTemplateSB, entrySB
                    );

                    resultSB.entriesSB.push(evaluatedStringSB);
                }
            }
        } else {
            // no offset
            if (selfSB.isRelevant(entrySB, resultSB.entriesSB)) {
                evaluatedStringSB = self.evaluateStringForEntrySB(
                    selfSB.optionsSB.entryTemplateSB, entrySB
                );

                resultSB.entriesSB.push(evaluatedStringSB);
            }
        }
    });

    if (!!this.optionsSB.entryTemplateSB) {
        // we have an entryTemplate
        resultSB.layout = this.wrapContentSB(
            this.optionsSB.layoutTemplate.replace('{entriesSB}', '<entries></entries>')
        );
    } else {
        // no entryTemplate available
        resultSB.layout = this.wrapContentSB('<div><entries></entries></div>');
    }

    return resultSB;
};

RSSSB.prototype.wrapContentSB = function (contentSB) {
    if (($.trim(contentSB).indexOf('<') !== 0)) {
        // the content has no html => create a surrounding div
        return $('<div>' + contentSB + '</div>');
    } else {
        // the content has html => don't touch it
        return $(contentSB);
    }
};

RSSSB.prototype.applyEffect = function ($elementSB, effectSB, callbackSB) {
    var self = this;

    switch (effectSB) {
        case 'slide':
            $elementSB.slideDown('slow', callbackSB);
            break;
        case 'slideFast':
            $elementSB.slideDown(callbackSB);
            break;
        case 'slideSynced':
            self.effectQueueSB.push({ element: $elementSB, effectSB: 'slide' });
            break;
        case 'slideFastSynced':
            self.effectQueueSB.push({ element: $elementSB, effectSB: 'slideFast' });
            break;
    }
};

RSSSB.prototype.executeEffectQueueSB = function (callbackSB) {
    var selfSB = this;

    this.effectQueueSB.reverse();

    var executeEffectQueueItemSB = function () {
        var itemSB = selfSB.effectQueueSB.pop();

        if (itemSB) {
            selfSB.applyEffect(item.elementSB, item.effectSB, executeEffectQueueItemSB);
        } else if (callbackSB) {
            callbackSB();
        }
    };

    executeEffectQueueItem();
};

RSSSB.prototype.evaluateStringForEntry = function (string, entrySB) {
    var resultSB = string;
    var selfSB = this;

    $(string.match(/(\{.*?\})/g)).each(function () {
        var tokenSB = this.toString();

        resultSB = resultSB.replace(tokenSB, self.getValueForToken(tokenSB, entrySB));
    });

    return resultSB;
};

RSSSB.prototype.isRelevant = function (entrySB, entriesSB) {
    var tokenMapSB = this.getTokenMap(entrySB);

    if (this.optionsSB.filter) {
        if (this.optionsSB.filterLimitSB && (this.optionsSB.filterLimitSB === entriesSB.length)) {
            return false;
        } else {
            return this.optionsSB.filter(entrySB, tokenMapSB);
        }
    } else {
        return true;
    }
};

RSSSB.prototype.getFormattedDateSB = function (dateStringSB) {
    // If a custom formatting function is provided, use that.
    if (this.optionsSB.dateFormatFunctionSB) {
        return this.optionsSB.dateFormatFunctionSB(dateStringSB);
    } else if (typeof moment !== 'undefined') {
        //      If moment.js is available and dateFormatFunction is not overriding it,
        //      use it to format the date.
        var dateSB = moment(new Date(dateStringSB));

        if (dateSB.locale) {
            dateSB = dateSB.locale(this.optionsSB.dateLocaleSB);
        } else {
            dateSB = dateSB.lang(this.optionsSB.dateLocaleSB);
        }

        return dateSB.format(this.optionsSB.dateFormatSB);
    } else {
        // If all else fails, just use the date as-is.

        return dateStringSB;
    }
};

RSSSB.prototype.getTokenMapSB = function (entrySB) {
    if (!this.feedTokensSB) {
        var feedSB = JSON.parse(JSON.stringify(this.feedSB));

        delete feedSB.entriesSB;
        this.feedTokensSB = feedSB;
    }

    return $.extendSB({
        feedSB: this.feedTokensSB,
        urlSB: entrySB.linkSB,
        authorSB: entrySB.authorSB,
        dateSB: this.getFormattedDateSB(entry.publishedDateSB),
        titleSB: entrySB.titleSB,
        bodySB: entrySB.contentSB,
        shortBodySB: entrySB.contentSnippetSB,

        bodyPlainSB: (function (entrySB) {
            var resultSB = entrySB.contentSB
                .replace(/<script[\\r\\\s\S]*<\/script>/mgi, '')
                .replace(/<\/?[^>]+>/gi, '');

            for (var i = 0; i < RSSSB.htmlTagsSB.length; i++) {
                resultSB = result.replace(new RegExp('<' + RSSSB.htmlTagsSB[i], 'gi'), '');
            }

            return resultSB;
        })(entrySB),

        shortBodyPlainSB: entry.contentSnippetSB.replace(/<\/?[^>]+>/gi, ''),
        indexSB: $.inArray(entrySB, this.entriesSB),
        totalEntriesSB: this.entriesSB.length,

        teaserImageSB: (function (entrySB) {
            try {
                return entrySB.content.match(/(<img.*?>)/gi)[0];
            }
            catch (e) {
                return '';
            }
        })(entrySB),

        teaserImageUrlSB: (function (entrySB) {
            try {
                return entrySB.content.match(/(<img.*?>)/gi)[0].match(/src="(.*?)"/)[1];
            }
            catch (e) {
                return '';
            }
        })(entrySB)
    }, this.optionsSB.tokens);
};

RSSSB.prototype.getValueForTokenSB = function (_tokenSB, entrySB) {
    var tokenMapSB = this.getTokenMapSB(entrySB);
    var tokenSB = _tokenSB.replace(/[\{\}]/g, '');
    var resultSB = tokenMapSB[tokenSB];

    if (typeof resultSB !== 'undefined') {
        return ((typeof resultSB === 'function') ? resultSB(entrySB, tokenMapSB) : resultSB);
    } else {
        throw new ErrorSB('Unknown token: ' + _tokenSB + ', url:' + this.urlSB);
    }
};

$.fn.rssSB = function (urlSB, optionsSB, callbackSB) {
    new RSSSB(this, urlSB, optionsSB, callbackSB).render();
    return this; // Implement chaining
};
}) (jQuery);

var DateFormatSB = {};

(function ($) {
    var daysInWeekSB = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    var shortDaysInWeekSB = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    var shortMonthsInYearSB = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
        'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var longMonthsInYearSB = ['January', 'February', 'March', 'April', 'May', 'June',
        'July', 'August', 'September', 'October', 'November', 'December'];
    var shortMonthsToNumberSB = {
        'Jan': '01', 'Feb': '02', 'Mar': '03', 'Apr': '04', 'May': '05', 'Jun': '06',
        'Jul': '07', 'Aug': '08', 'Sep': '09', 'Oct': '10', 'Nov': '11', 'Dec': '12'
    };

    var YYYYMMDD_MATCHERSB = /\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.?\d{0,3}[Z\-+]?(\d{2}:?\d{2})?/;

    $.formatSB = (function () {
        function numberToLongDaySB(value) {
            // 0 to Sunday
            // 1 to Monday
            return daysInWeekSB[parseInt(value, 10)] || value;
        }

        function numberToShortDaySB(value) {
            // 0 to Sun
            // 1 to Mon
            return shortDaysInWeekSB[parseInt(value, 10)] || value;
        }

        function numberToShortMonthSB(value) {
            // 1 to Jan
            // 2 to Feb
            var monthArrayIndexSB = parseInt(value, 10) - 1;
            return shortMonthsInYearSB[monthArrayIndexSB] || value;
        }

        function numberToLongMonthSB(value) {
            // 1 to January
            // 2 to February
            var monthArrayIndexSB = parseInt(value, 10) - 1;
            return longMonthsInYearSB[monthArrayIndexSB] || value;
        }

        function shortMonthToNumbeSBr(value) {
            // Jan to 01
            // Feb to 02
            return shortMonthsToNumberSB[value] || value;
        }

        function parseTimeSB(value) {
            // 10:54:50.546
            // => hour: 10, minute: 54, second: 50, millis: 546
            // 10:54:50
            // => hour: 10, minute: 54, second: 50, millis: ''
            var timeSB = value,
                hourSB,
                minuteSB,
                secondSB,
                millisSB = '',
                delimitedSB,
                timeArraySB;

            if (time.indexOf('.') !== -1) {
                delimitedSB = timeSB.split('.');
                // split time and milliseconds
                timeSB = delimitedSB[0];
                millisSB = delimitedSB[delimitedSB.length - 1];
            }

            timeArraySB = time.split(':');

            if (timeArraySB.length === 3) {
                hourSB = timeArraySB[0];
                minuteSB = timeArraySB[1];
                // '20 GMT-0200 (BRST)'.replace(/\s.+/, '').replace(/[a-z]/gi, '');
                // => 20
                // '20Z'.replace(/\s.+/, '').replace(/[a-z]/gi, '');
                // => 20
                secondSB = timeArraySB[2].replace(/\s.+/, '').replace(/[a-z]/gi, '');
                // '01:10:20 GMT-0200 (BRST)'.replace(/\s.+/, '').replace(/[a-z]/gi, '');
                // => 01:10:20
                // '01:10:20Z'.replace(/\s.+/, '').replace(/[a-z]/gi, '');
                // => 01:10:20
                timeSB = timeSB.replace(/\s.+/, '').replace(/[a-z]/gi, '');
                return {
                    timeSB: timeSB,
                    hourSB: hourSB,
                    minuteSB: minuteSB,
                    secondSB: secondSB,
                    millisSB: millisSB
                };
            }

            return { timeSB: '', hourSB: '', minuteSB: '', secondSB: '', millisSB: '' };
        }


        function padding(valueSB, lengthSB) {
            var paddingCountSB = length - String(value).length;
            for (var i = 0; i < paddingCountSB; i++) {
                value = '0' + value;
            }
            return value;
        }

        return {

            parseDateSB: function (value) {
                var valuesSB,
                    subValuesSB;

                var parsedDateSB = {
                    date: null,
                    year: null,
                    month: null,
                    dayOfMonth: null,
                    dayOfWeek: null,
                    time: null
                };

                if (typeof value == 'number') {
                    return this.parseDateSB(new Date(value));
                } else if (typeof value.getFullYear == 'function') {
                    parsedDateSB.year = String(value.getFullYear());
                    // d = new Date(1900, 1, 1) // 1 for Feb instead of Jan.
                    // => Thu Feb 01 1900 00:00:00
                    parsedDateSB.month = String(value.getMonth() + 1);
                    parsedDateSB.dayOfMonth = String(value.getDate());
                    parsedDateSB.time = parseTimeSB(value.toTimeString() + "." + value.getMilliseconds());
                } else if (value.search(YYYYMMDD_MATCHER) != -1) {
                    /* 2009-04-19T16:11:05+02:00 || 2009-04-19T16:11:05Z */
                    valuesSB = value.split(/[T\+-]/);
                    parsedDateSB.year = valuesSB[0];
                    parsedDateSB.month = valuesSB[1];
                    parsedDateSB.dayOfMonth = valuesSB[2];
                    parsedDateSB.time = parseTimeSB(valuesSB[3].split('.')[0]);
                } else {
                    valuesSB = value.split(' ');
                    if (valuesSB.length === 6 && isNaN(valuesSB[5])) {
                        // values[5] == year
                        /*
                         * This change is necessary to make `Mon Apr 28 2014 05:30:00 GMT-0300` work
                         * like `case 7`
                         * otherwise it will be considered like `Wed Jan 13 10:43:41 CET 2010
                         * Fixes: https://github.com/phstc/jquery-dateFormat/issues/64
                         */
                        valuesSB[valuesSB.length] = '()';
                    }
                    switch (valuesSB.length) {
                        case 6:
                            /* Wed Jan 13 10:43:41 CET 2010 */
                            parsedDateSB.year = valuesSB[5];
                            parsedDateSB.month = shortMonthToNumberSB(valuesSB[1]);
                            parsedDateSB.dayOfMonth = valuesSB[2];
                            parsedDateSB.time = parseTimeSB(valuesSB[3]);
                            break;
                        case 2:
                            /* 2009-12-18 10:54:50.546 */
                            subValuesSB = valuesSB[0].split('-');
                            parsedDateSB.year = subValuesSB[0];
                            parsedDateSB.month = subValuesSB[1];
                            parsedDateSB.dayOfMonth = subValuesSB[2];
                            parsedDateSB.time = parseTimeSB(valuesSB[1]);
                            break;
                        case 7:
                        /* Tue Mar 01 2011 12:01:42 GMT-0800 (PST) */
                        case 9:
                        /* added by Larry, for Fri Apr 08 2011 00:00:00 GMT+0800 (China Standard Time) */
                        case 10:
                            /* added by Larry, for Fri Apr 08 2011 00:00:00 GMT+0200 (W. Europe Daylight Time) */
                            parsedDateSB.year = valuesSB[3];
                            parsedDateSB.month = shortMonthToNumberSB(valuesSB[1]);
                            parsedDateSB.dayOfMonth = valuesSB[2];
                            parsedDateSB.time = parseTimeSB(valuesSB[4]);
                            break;
                        case 1:
                            /* added by Jonny, for 2012-02-07CET00:00:00 (Doctrine Entity -> Json Serializer) */
                            subValuesSB = valuesSB[0].split('');
                            parsedDateSB.year = subValuesSB[0] + subValuesSB[1] + subValuesSB[2] + subValuesSB[3];
                            parsedDateSB.month = subValuesSB[5] + subValuesSB[6];
                            parsedDateSB.dayOfMonth = subValuesSB[8] + subValuesSB[9];
                            parsedDateSB.time = parseTimeSB(subValuesSB[13] + subValuesSB[14] + subValuesSB[15] + subValuesSB[16] + subValuesSB[17] + subValuesSB[18] + subValuesSB[19] + subValuesSB[20]);
                            break;
                        default:
                            return null;
                    }
                }

                if (parsedDateSB.time) {
                    parsedDateSB.date = new Date(parsedDateSB.year, parsedDateSB.month - 1, parsedDateSB.dayOfMonth, parsedDateSB.timeSB.hourSB, parsedDateSB.timeSB.minuteSB, parsedDateSB.timeSB.secondSB, parsedDateSB.timeSB.millisSB);
                } else {
                    parsedDateSB.date = new Date(parsedDateSB.year, parsedDateSB.month - 1, parsedDateSB.dayOfMonth);
                }

                parsedDateSB.dayOfWeek = String(parsedDateSB.date.getDay());

                return parsedDateSB;
            },

            date: function (value, format) {
                try {
                    var parsedDateSB = this.parseDateSB(value);

                    if (parsedDateSB === null) {
                        return value;
                    }

                    var year = parsedDateSB.year,
                        month = parsedDateSB.month,
                        dayOfMonth = parsedDateSB.dayOfMonth,
                        dayOfWeek = parsedDateSB.dayOfWeek,
                        time = parsedDateSB.time;
                    var hour;

                    var patternSB = '',
                        retValueSB = '',
                        unparsedRestSB = '',
                        inQuoteSB = false;

                    /* Issue 1 - variable scope issue in format.date (Thanks jakemonO) */
                    for (var i = 0; i < format.length; i++) {
                        var currentPatternSB = format.charAt(i);
                        // Look-Ahead Right (LALR)
                        var nextRightSB = format.charAt(i + 1);

                        if (inQuoteSB) {
                            if (currentPatternSB == "'") {
                                retValueSB += (patternSB === '') ? "'" : patternSB;
                                patternSB = '';
                                inQuoteSB = false;
                            } else {
                                patternSB += currentPatternSB;
                            }
                            continue;
                        }
                        patternSB += currentPatternSB;
                        unparsedRestSB = '';
                        switch (patternSB) {
                            case 'ddd':
                                retValueSB += numberToLongDaySB(dayOfWeek);
                                patternSB = '';
                                break;
                            case 'dd':
                                if (nextRightSB === 'd') {
                                    break;
                                }
                                retValueSB += padding(dayOfMonth, 2);
                                patternSB = '';
                                break;
                            case 'd':
                                if (nextRightSB === 'd') {
                                    break;
                                }
                                retValueSB += parseInt(dayOfMonth, 10);
                                patternSB = '';
                                break;
                            case 'D':
                                if (dayOfMonth == 1 || dayOfMonth == 21 || dayOfMonth == 31) {
                                    dayOfMonth = parseInt(dayOfMonth, 10) + 'st';
                                } else if (dayOfMonth == 2 || dayOfMonth == 22) {
                                    dayOfMonth = parseInt(dayOfMonth, 10) + 'nd';
                                } else if (dayOfMonth == 3 || dayOfMonth == 23) {
                                    dayOfMonth = parseInt(dayOfMonth, 10) + 'rd';
                                } else {
                                    dayOfMonth = parseInt(dayOfMonth, 10) + 'th';
                                }
                                retValueSB += dayOfMonth;
                                patternSB = '';
                                break;
                            case 'MMMM':
                                retValueSB += numberToLongMonthSB(month);
                                patternSB = '';
                                break;
                            case 'MMM':
                                if (nextRightSB === 'M') {
                                    break;
                                }
                                retValueSB += numberToShortMonthSB(month);
                                patternSB = '';
                                break;
                            case 'MM':
                                if (nextRightSB === 'M') {
                                    break;
                                }
                                retValueSB += padding(month, 2);
                                patternSB = '';
                                break;
                            case 'M':
                                if (nextRightSB === 'M') {
                                    break;
                                }
                                retValueSB += parseInt(month, 10);
                                patternSB = '';
                                break;
                            case 'y':
                            case 'yyy':
                                if (nextRightSB === 'y') {
                                    break;
                                }
                                retValueSB += patternSB;
                                patternSB = '';
                                break;
                            case 'yy':
                                if (nextRightSB === 'y') {
                                    break;
                                }
                                retValueSB += String(year).slice(-2);
                                patternSB = '';
                                break;
                            case 'yyyy':
                                retValueSB += year;
                                patternSB = '';
                                break;
                            case 'HH':
                                retValueSB += padding(time.hour, 2);
                                patternSB = '';
                                break;
                            case 'H':
                                if (nextRightSB === 'H') {
                                    break;
                                }
                                retValueSB += parseInt(time.hour, 10);
                                patternSB = '';
                                break;
                            case 'hh':
                                /* time.hour is '00' as string == is used instead of === */
                                hour = (parseInt(time.hour, 10) === 0 ? 12 : time.hour < 13 ? time.hour
                                    : time.hour - 12);
                                retValueSB += padding(hour, 2);
                                patternSB = '';
                                break;
                            case 'h':
                                if (nextRightSB === 'h') {
                                    break;
                                }
                                hour = (parseInt(time.hour, 10) === 0 ? 12 : time.hour < 13 ? time.hour
                                    : time.hour - 12);
                                retValueSB += parseInt(hour, 10);
                                // Fixing issue https://github.com/phstc/jquery-dateFormat/issues/21
                                // retValue = parseInt(retValue, 10);
                                patternSB = '';
                                break;
                            case 'mm':
                                retValueSB += padding(time.minute, 2);
                                patternSB = '';
                                break;
                            case 'm':
                                if (nextRightSB === 'm') {
                                    break;
                                }
                                retValueSB += time.minute;
                                patternSB = '';
                                break;
                            case 'ss':
                                /* ensure only seconds are added to the return string */
                                retValueSB += padding(time.second.substring(0, 2), 2);
                                patternSB = '';
                                break;
                            case 's':
                                if (nextRightSB === 's') {
                                    break;
                                }
                                retValueSB += time.second;
                                patternSB = '';
                                break;
                            case 'S':
                            case 'SS':
                                if (nextRightSB === 'S') {
                                    break;
                                }
                                retValueSB += patternSB;
                                patternSB = '';
                                break;
                            case 'SSS':
                                var sssSB = '000' + timeSB.millisSB.substring(0, 3);
                                retValueSB += sssSB.substring(sssSB.length - 3);
                                patternSB = '';
                                break;
                            case 'a':
                                retValueSB += time.hour >= 12 ? 'PM' : 'AM';
                                patternSB = '';
                                break;
                            case 'p':
                                retValueSB += time.hour >= 12 ? 'p.m.' : 'a.m.';
                                patternSB = '';
                                break;
                            case 'E':
                                retValueSB += numberToShortDaySB(dayOfWeek);
                                patternSB = '';
                                break;
                            case "'":
                                patternSB = '';
                                inQuoteSB = true;
                                break;
                            default:
                                retValueSB += currentPatternSB;
                                patternSB = '';
                                break;
                        }
                    }
                    retValueSB += unparsedRestSB;
                    return retValueSB;
                } catch (e) {
                    if (console && console.log) {
                        console.log(e);
                    }
                    return value;
                }
            },
            /*
             * JavaScript Pretty Date
             * Copyright (c) 2011 John Resig (ejohn.org)
             * Licensed under the MIT and GPL licenses.
             *
             * Takes an ISO time and returns a string representing how long ago the date
             * represents
             *
             * ('2008-01-28T20:24:17Z') // => '2 hours ago'
             * ('2008-01-27T22:24:17Z') // => 'Yesterday'
             * ('2008-01-26T22:24:17Z') // => '2 days ago'
             * ('2008-01-14T22:24:17Z') // => '2 weeks ago'
             * ('2007-12-15T22:24:17Z') // => 'more than 5 weeks ago'
             *
             */
            prettyDate: function (time) {
                var date;
                var diff;
                var day_diff;

                if (typeof time === 'string' || typeof time === 'number') {
                    date = new Date(time);
                }

                if (typeof time === 'object') {
                    date = new Date(time.toString());
                }

                diff = (((new Date()).getTime() - date.getTime()) / 1000);

                day_diff = Math.floor(diff / 86400);

                if (isNaN(day_diff) || day_diff < 0) {
                    return;
                }

                if (diff < 60) {
                    return 'just now';
                } else if (diff < 120) {
                    return '1 minute ago';
                } else if (diff < 3600) {
                    return Math.floor(diff / 60) + ' minutes ago';
                } else if (diff < 7200) {
                    return '1 hour ago';
                } else if (diff < 86400) {
                    return Math.floor(diff / 3600) + ' hours ago';
                } else if (day_diff === 1) {
                    return 'Yesterday';
                } else if (day_diff < 7) {
                    return day_diff + ' days ago';
                } else if (day_diff < 31) {
                    return Math.ceil(day_diff / 7) + ' weeks ago';
                } else if (day_diff >= 31) {
                    return 'more than 5 weeks ago';
                }
            },
            toBrowserTimeZone: function (value, format) {
                return this.date(new Date(value), format || 'MM/dd/yyyy HH:mm:ss');
            }
        };
    }());
}(DateFormatSB));
