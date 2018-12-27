import log4js from "log4js";
const logger = log4js.getLogger();
logger.level = "debug";

export default {
    debug: (...arg) => {
        logger.debug(...arg);
    },
    info: (...arg) => {
        logger.info(...arg);
    }
};
