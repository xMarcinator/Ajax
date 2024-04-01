const webpack = require("webpack");

const { merge } = require("webpack-merge");
const common = require("./webpack.common.js");

module.exports = merge(common, {
    mode: "development",
    devtool: "inline-source-map",
    devServer: {
        static: "./dist",
        compress: true,
        port: 5500,
    },
    plugins: [
        new webpack.DefinePlugin({
            API_PREFIX:  JSON.stringify("http://localhost:5000"),
        })
    ]
});
