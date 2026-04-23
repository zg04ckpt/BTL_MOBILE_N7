package com.hoangcn.quizbattle.battles.models;

import java.util.ArrayList;
import java.util.List;

public class MatchQuestionContentItem {
    private int id;
    private String stringContent;
    private String imageUrl;
    private String audioUrl;
    private String videoUrl;
    private int level;
    private String topicName;
    private List<String> stringAnswers = new ArrayList<>();
    private List<String> correctAnswers = new ArrayList<>();

    public int getId() {
        return id;
    }

    public String getStringContent() {
        return stringContent;
    }

    public String getImageUrl() {
        return imageUrl;
    }

    public String getAudioUrl() {
        return audioUrl;
    }

    public String getVideoUrl() {
        return videoUrl;
    }

    public int getLevel() {
        return level;
    }

    public String getTopicName() {
        return topicName;
    }

    public List<String> getStringAnswers() {
        return stringAnswers;
    }

    public List<String> getCorrectAnswers() {
        return correctAnswers;
    }
}
