var gulp = require('gulp');
var ts = require('gulp-typescript');
var sass = require('gulp-sass');

gulp.task("scripts", function() {
	var tsProject = ts.createProject('src/tsconfig.json');
	return gulp.src('src/**/*.ts')
		.pipe(tsProject())
		.pipe(gulp.dest('src'));
});

gulp.task("styles", function() {
	return gulp.src('src/**/*.scss')
		.pipe(sass({outputStyle: 'compressed'}).on('error', sass.logError))
		.pipe(gulp.dest('src'));
});

gulp.task('default', function() {
	gulp.watch('src/**/*.ts', ['scripts']);
	gulp.watch('src/**/*.scss', ['styles']);
});
