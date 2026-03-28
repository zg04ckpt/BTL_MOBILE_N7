plugins {
    alias(libs.plugins.android.application)
}

val envVars = mutableMapOf<String, String>()
val envFile = rootProject.file(".env")
if (envFile.exists()) {
    envFile.readLines().forEach { line ->
        val raw = line.trim()
        if (raw.isEmpty() || raw.startsWith("#") || !raw.contains("=")) {
            return@forEach
        }
        val idx = raw.indexOf("=")
        val key = raw.substring(0, idx).trim()
        val value = raw.substring(idx + 1).trim()
        if (key.isNotEmpty()) {
            envVars[key] = value
        }
    }
}

val apiBaseUrl = envVars.getValue("API_BASE_URL").trim()

android {
    namespace = "com.n7.quizbattle"
    compileSdk {
        version = release(36)
    }

    defaultConfig {
        applicationId = "com.n7.quizbattle"
        minSdk = 24
        targetSdk = 36
        versionCode = 1
        versionName = "1.0"

        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
        buildConfigField("String", "API_BASE_URL", "\"$apiBaseUrl\"")
    }

    buildFeatures {
        buildConfig = true
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(
                getDefaultProguardFile("proguard-android-optimize.txt"),
                "proguard-rules.pro"
            )
        }
    }
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_11
        targetCompatibility = JavaVersion.VERSION_11
    }
}

dependencies {
    implementation(libs.appcompat)
    implementation(libs.material)
    implementation(libs.activity)
    implementation(libs.constraintlayout)
    implementation("androidx.cardview:cardview:1.0.0")
    implementation("androidx.recyclerview:recyclerview:1.4.0")
    testImplementation(libs.junit)
    androidTestImplementation(libs.ext.junit)
    androidTestImplementation(libs.espresso.core)

    implementation("com.squareup.retrofit2:retrofit:2.9.0")
    implementation("com.squareup.retrofit2:converter-gson:2.9.0")
}