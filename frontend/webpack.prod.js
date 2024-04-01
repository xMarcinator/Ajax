const webpack = require("webpack");


const { merge } = require("webpack-merge");
const common = require("./webpack.common.js");

module.exports = merge(common, {
    mode: "production",
    optimization: {
        splitChunks: {
            chunks: "all",
        },
    },
    output: {
        filename: "static/js/[name].[contenthash].js",
        chunkFilename: "static/js/[name].[contenthash].js",
        assetModuleFilename: "static/[hash][ext][query]",
    },
    plugins: [
        new webpack.DefinePlugin({
            API_PREFIX: JSON.stringify("/api"),
        })
    ]
    
});
