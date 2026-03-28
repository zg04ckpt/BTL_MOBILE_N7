package com.hoangcn.quizbattle.lobby.models;

import java.util.List;

public class StartOptionsResponse {
    private List<OptionItem> typesOfBattle;
    private List<OptionItem> contentTypes;
    private List<Integer> numberOfPlayers;
    private List<TopicItem> topicsOfContent;

    public List<OptionItem> getTypesOfBattle() {
        return typesOfBattle;
    }

    public void setTypesOfBattle(List<OptionItem> typesOfBattle) {
        this.typesOfBattle = typesOfBattle;
    }

    public List<TopicItem> getTopicsOfContent() {
        return topicsOfContent;
    }

    public void setTopicsOfContent(List<TopicItem> topicsOfContent) {
        this.topicsOfContent = topicsOfContent;
    }

    public List<Integer> getNumberOfPlayers() {
        return numberOfPlayers;
    }

    public void setNumberOfPlayers(List<Integer> numberOfPlayers) {
        this.numberOfPlayers = numberOfPlayers;
    }

    public List<OptionItem> getContentTypes() {
        return contentTypes;
    }

    public void setContentTypes(List<OptionItem> contentTypes) {
        this.contentTypes = contentTypes;
    }
}
