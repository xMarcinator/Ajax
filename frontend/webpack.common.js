const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const autoprefixer = require("autoprefixer");

module.exports = {
    entry: {
        //vendor: "./src/js/vendor.js",
        app: "./src/js/index.js",
    },
    plugins: [
        //if usage of template is needed @see https://github.com/jantimon/html-webpack-plugin/blob/main/docs/template-option.md
        new HtmlWebpackPlugin({
            title: "Test App",
            template: "./src/index.html",
        }),
        new MiniCssExtractPlugin({
            filename: "static/css/[name].css",
        }),
    ],
    module: {
        rules: [
            {
                test: /\.(png|svg|jpg|jpeg|gif)$/i,
                type: "asset/resource",
                generator: {
                    filename: "static/img/[hash][ext][query]",
                },
            },
            {
                test: /\.(woff(2)?|ttf|eot|svg)(\?v=\d+\.\d+\.\d+)?$/,
                type: "asset/resource",
                generator: {
                    filename: "static/font/[hash][ext][query]",
                },
            },
            {
                test: /\.(s?css)$/,
                use: [
                    // extract css into files
                    MiniCssExtractPlugin.loader,
                    "css-loader",
                    {
                        // Loader for webpack to process CSS with PostCSS
                        loader: "postcss-loader",
                        options: {
                            postcssOptions: {
                                plugins: [autoprefixer],
                            },
                        },
                    },
                    "sass-loader",
                ],
            },
        ],
    },
    output: {
        filename: "static/js/[name].bundle.js",
        path: path.resolve(__dirname, "dist"),
        assetModuleFilename: "static/[hash][ext][query]",
        clean: true,
    },
};
